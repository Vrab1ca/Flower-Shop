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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IFlowerRepository flowerRepository,
            IEmailService emailService,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _flowerRepository = flowerRepository;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(OrderCreateViewModel model)
        {
            if (model.Items == null || !model.Items.Any())
            {
                throw new InvalidOperationException("Order must contain at least one item.");
            }

            var order = new Order
            {
                CustomerId = model.CustomerId,
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            decimal totalPrice = 0;

            foreach (var item in model.Items)
            {
                var flower = await _flowerRepository.GetByIdAsync(item.FlowerId);

                if (flower == null)
                {
                    throw new InvalidOperationException("Flower not found.");
                }

                if (flower.AvailableQuantity < item.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Not enough stock for {flower.Name}.");
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

            await _emailService.SendOrderConfirmationAsync(order.OrderId);

            return order.OrderId;
        }

        public async Task<OrderDetailsViewModel?> GetOrderDetailsAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithItemsAsync(orderId);

            if (order == null)
            {
                return null;
            }

            return _mapper.Map<OrderDetailsViewModel>(order);
        }
    }
}