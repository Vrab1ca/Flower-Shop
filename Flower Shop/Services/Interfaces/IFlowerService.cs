using FlowerShopOnlineOrderSystem.ViewModels;

namespace FlowerShopOnlineOrderSystem.Services.Interfaces
{
    public interface IFlowerService
    {
        Task<IReadOnlyCollection<FlowerViewModel>> GetCatalogAsync();
        Task<FlowerViewModel?> GetByIdAsync(int id);
        Task CreateAsync(FlowerViewModel model);
        Task<bool> UpdateAsync(FlowerViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
