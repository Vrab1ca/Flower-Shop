using FlowerShopOnlineOrderSystem.Models;

namespace FlowerShopOnlineOrderSystem.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IReadOnlyCollection<Order>> GetAllWithCustomersAsync();
        Task<Order?> GetWithItemsAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task SaveChangesAsync();
    }
}
