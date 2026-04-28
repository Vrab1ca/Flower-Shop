using FlowerShopOnlineOrderSystem.Models;

namespace FlowerShopOnlineOrderSystem.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(Order order);
        Task SendOrderStatusUpdateAsync(Order order);
    }
}
