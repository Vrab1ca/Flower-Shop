using FlowerShopOnlineOrderSystem.ViewModels;

namespace FlowerShopOnlineOrderSystem.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(OrderCreateViewModel model);
        Task<OrderDetailsViewModel?> GetOrderDetailsAsync(int orderId);
    }
}