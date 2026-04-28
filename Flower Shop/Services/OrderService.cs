using AutoMapper;
using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.Repositories.Interfaces;
using FlowerShopOnlineOrderSystem.Services.Interfaces;
using FlowerShopOnlineOrderSystem.ViewModels;

namespace FlowerShopOnlineOrderSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFlowerRepository _flowerRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IFlowerRepository flowerRepository,
            ICustomerRepository customerRepository,
            IEmailService emailService,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _flowerRepository = flowerRepository;
            _customerRepository = customerRepository;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<OrderCreateViewModel> BuildCreateModelAsync(OrderCreateViewModel? model = null)
        {
            var flowers = await _flowerRepository.GetAvailableAsync();
            var quantities = model?.Items.ToDictionary(i => i.FlowerId, i => i.Quantity)
                ?? new Dictionary<int, int>();

            var result = model ?? new OrderCreateViewModel();
            result.Items = flowers
                .Select(f =>
                {
                    var item = _mapper.Map<OrderItemInputViewModel>(f);
                    item.Quantity = quantities.GetValueOrDefault(f.FlowerId);
                    return item;
                })
                .ToList();

            return result;
        }

        public async Task<int> CreateOrderAsync(OrderCreateViewModel model)
        {
            var selectedItems = model.Items
                .Where(i => i.Quantity > 0)
                .ToList();

            if (selectedItems.Count == 0)
            {
                throw new InvalidOperationException("Order must contain at least one item.");
            }

            var normalizedEmail = model.CustomerEmail.Trim().ToLowerInvariant();
            var customer = await _customerRepository.GetByEmailAsync(normalizedEmail);

            if (customer == null)
            {
                customer = _mapper.Map<Customer>(model);
                await _customerRepository.AddAsync(customer);
            }

            var order = new Order
            {
                Customer = customer,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            decimal totalPrice = 0;

            foreach (var item in selectedItems)
            {
                var flower = await _flowerRepository.GetByIdAsync(item.FlowerId);

                if (flower == null)
                {
                    throw new InvalidOperationException("Flower not found.");
                }

                if (flower.AvailableQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Not enough stock for {flower.Name}.");
                }

                var subtotal = flower.Price * item.Quantity;
                flower.AvailableQuantity -= item.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    FlowerId = flower.FlowerId,
                    Quantity = item.Quantity,
                    Subtotal = subtotal
                });

                totalPrice += subtotal;
                await _flowerRepository.UpdateAsync(flower);
            }

            order.TotalPrice = totalPrice;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            await _emailService.SendOrderConfirmationAsync(order);

            return order.OrderId;
        }

        public async Task<OrderDetailsViewModel?> GetOrderDetailsAsync(int orderId)
        {
            var order = await _orderRepository.GetWithItemsAsync(orderId);
            return order == null ? null : _mapper.Map<OrderDetailsViewModel>(order);
        }

        public async Task<IReadOnlyCollection<OrderListViewModel>> GetOrdersAsync()
        {
            var orders = await _orderRepository.GetAllWithCustomersAsync();
            return _mapper.Map<IReadOnlyCollection<OrderListViewModel>>(orders);
        }

        public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetWithItemsAsync(orderId);

            if (order == null)
            {
                return false;
            }

            order.Status = status;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
            await _emailService.SendOrderStatusUpdateAsync(order);
            return true;
        }
    }
}
