using FlowerShopOnlineOrderSystem.Models;

namespace FlowerShopOnlineOrderSystem.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task AddAsync(Customer customer);
    }
}
