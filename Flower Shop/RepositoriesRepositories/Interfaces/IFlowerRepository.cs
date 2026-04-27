using FlowerShopOnlineOrderSystem.Models;

namespace FlowerShopOnlineOrderSystem.Repositories.Interfaces
{
    public interface IFlowerRepository
    {
        Task<IEnumerable<Flower>> GetAllAsync();
        Task<Flower?> GetByIdAsync(int id);
        Task AddAsync(Flower flower);
        Task UpdateAsync(Flower flower);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}