using AutoMapper;
using FlowerShopOnlineOrderSystem.Mapping;
using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.Repositories.Interfaces;
using FlowerShopOnlineOrderSystem.Services;
using FlowerShopOnlineOrderSystem.Services.Interfaces;
using FlowerShopOnlineOrderSystem.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Flower_Shop.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_DecrementsInventoryAndCalculatesTotal()
        {
            var flower = new Flower
            {
                FlowerId = 1,
                Name = "Rose Bouquet",
                Description = "Red roses",
                Price = 12.50m,
                AvailableQuantity = 10
            };
            var orderRepository = new FakeOrderRepository();
            var emailService = new FakeEmailService();
            var service = CreateService(new[] { flower }, orderRepository, emailService);

            var orderId = await service.CreateOrderAsync(new OrderCreateViewModel
            {
                CustomerFirstName = "Ada",
                CustomerLastName = "Lovelace",
                CustomerEmail = "ADA@example.com",
                Items = new List<OrderItemInputViewModel>
                {
                    new() { FlowerId = 1, Quantity = 3 }
                }
            });

            var order = Assert.Single(orderRepository.Orders);
            Assert.Equal(100, orderId);
            Assert.Equal(7, flower.AvailableQuantity);
            Assert.Equal(37.50m, order.TotalPrice);
            Assert.Equal("ada@example.com", order.Customer?.Email);
            Assert.Equal(1, emailService.ConfirmationCount);
        }

        [Fact]
        public async Task CreateOrderAsync_ThrowsWhenInventoryIsInsufficient()
        {
            var flower = new Flower
            {
                FlowerId = 1,
                Name = "Tulip Mix",
                Description = "Tulips",
                Price = 8.00m,
                AvailableQuantity = 2
            };
            var orderRepository = new FakeOrderRepository();
            var emailService = new FakeEmailService();
            var service = CreateService(new[] { flower }, orderRepository, emailService);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CreateOrderAsync(new OrderCreateViewModel
                {
                    CustomerFirstName = "Grace",
                    CustomerLastName = "Hopper",
                    CustomerEmail = "grace@example.com",
                    Items = new List<OrderItemInputViewModel>
                    {
                        new() { FlowerId = 1, Quantity = 3 }
                    }
                }));

            Assert.Empty(orderRepository.Orders);
            Assert.Equal(2, flower.AvailableQuantity);
            Assert.Equal(0, emailService.ConfirmationCount);
        }

        private static OrderService CreateService(
            IEnumerable<Flower> flowers,
            IOrderRepository orderRepository,
            IEmailService emailService)
        {
            var mapper = new MapperConfiguration(
                    config => config.AddProfile<AutoMapperProfile>(),
                    NullLoggerFactory.Instance)
                .CreateMapper();

            return new OrderService(
                orderRepository,
                new FakeFlowerRepository(flowers),
                new FakeCustomerRepository(),
                emailService,
                mapper);
        }

        private sealed class FakeFlowerRepository : IFlowerRepository
        {
            private readonly List<Flower> _flowers;

            public FakeFlowerRepository(IEnumerable<Flower> flowers)
            {
                _flowers = flowers.ToList();
            }

            public Task AddAsync(Flower flower)
            {
                _flowers.Add(flower);
                return Task.CompletedTask;
            }

            public Task DeleteAsync(int id)
            {
                _flowers.RemoveAll(f => f.FlowerId == id);
                return Task.CompletedTask;
            }

            public Task<IReadOnlyCollection<Flower>> GetAllAsync()
            {
                return Task.FromResult<IReadOnlyCollection<Flower>>(_flowers);
            }

            public Task<IReadOnlyCollection<Flower>> GetAvailableAsync()
            {
                return Task.FromResult<IReadOnlyCollection<Flower>>(
                    _flowers.Where(f => f.AvailableQuantity > 0).ToList());
            }

            public Task<Flower?> GetByIdAsync(int id)
            {
                return Task.FromResult(_flowers.FirstOrDefault(f => f.FlowerId == id));
            }

            public Task SaveChangesAsync()
            {
                return Task.CompletedTask;
            }

            public Task UpdateAsync(Flower flower)
            {
                return Task.CompletedTask;
            }
        }

        private sealed class FakeCustomerRepository : ICustomerRepository
        {
            private readonly List<Customer> _customers = new();

            public Task AddAsync(Customer customer)
            {
                customer.CustomerId = _customers.Count + 1;
                _customers.Add(customer);
                return Task.CompletedTask;
            }

            public Task<Customer?> GetByEmailAsync(string email)
            {
                return Task.FromResult(_customers.FirstOrDefault(c => c.Email == email));
            }
        }

        private sealed class FakeOrderRepository : IOrderRepository
        {
            public List<Order> Orders { get; } = new();

            public Task AddAsync(Order order)
            {
                order.OrderId = 100 + Orders.Count;
                Orders.Add(order);
                return Task.CompletedTask;
            }

            public Task<IReadOnlyCollection<Order>> GetAllWithCustomersAsync()
            {
                return Task.FromResult<IReadOnlyCollection<Order>>(Orders);
            }

            public Task<Order?> GetWithItemsAsync(int id)
            {
                return Task.FromResult(Orders.FirstOrDefault(o => o.OrderId == id));
            }

            public Task SaveChangesAsync()
            {
                return Task.CompletedTask;
            }

            public Task UpdateAsync(Order order)
            {
                return Task.CompletedTask;
            }
        }

        private sealed class FakeEmailService : IEmailService
        {
            public int ConfirmationCount { get; private set; }

            public Task SendOrderConfirmationAsync(Order order)
            {
                ConfirmationCount++;
                return Task.CompletedTask;
            }

            public Task SendOrderStatusUpdateAsync(Order order)
            {
                return Task.CompletedTask;
            }
        }
    }
}
