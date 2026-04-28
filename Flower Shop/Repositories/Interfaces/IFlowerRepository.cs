using FlowerShopOnlineOrderSystem.Models;

namespace FlowerShopOnlineOrderSystem.Repositories.Interfaces
{
    public interface IFlowerRepository
    {
        Task<IReadOnlyCollection<Flower>> GetAllAsync();
        Task<IReadOnlyCollection<Flower>> GetAvailableAsync();
        Task<Flower?> GetByIdAsync(int id);
        Task AddAsync(Flower flower);
        Task UpdateAsync(Flower flower);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
