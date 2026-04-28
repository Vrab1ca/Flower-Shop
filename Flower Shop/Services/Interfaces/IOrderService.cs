using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.ViewModels;

namespace FlowerShopOnlineOrderSystem.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderCreateViewModel> BuildCreateModelAsync(OrderCreateViewModel? model = null);
        Task<int> CreateOrderAsync(OrderCreateViewModel model);
        Task<OrderDetailsViewModel?> GetOrderDetailsAsync(int orderId);
        Task<IReadOnlyCollection<OrderListViewModel>> GetOrdersAsync();
        Task<bool> UpdateStatusAsync(int orderId, OrderStatus status);
    }
}
