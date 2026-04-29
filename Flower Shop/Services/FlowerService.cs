using AutoMapper;
using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.Repositories.Interfaces;
using FlowerShopOnlineOrderSystem.Services.Interfaces;
using FlowerShopOnlineOrderSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopOnlineOrderSystem.Services
{
    public class FlowerService : IFlowerService
    {
        private readonly IFlowerRepository _flowerRepository;
        private readonly IMapper _mapper;

        public FlowerService(IFlowerRepository flowerRepository, IMapper mapper)
        {
            _flowerRepository = flowerRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<FlowerViewModel>> GetCatalogAsync()
        {
            var flowers = await _flowerRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyCollection<FlowerViewModel>>(flowers);
        }

        public async Task<FlowerViewModel?> GetByIdAsync(int id)
        {
            var flower = await _flowerRepository.GetByIdAsync(id);
            return flower == null ? null : _mapper.Map<FlowerViewModel>(flower);
        }

        public async Task CreateAsync(FlowerViewModel model)
        {
            var flower = _mapper.Map<Flower>(model);
            await _flowerRepository.AddAsync(flower);
            await _flowerRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(FlowerViewModel model)
        {
            var flower = await _flowerRepository.GetByIdAsync(model.FlowerId);

            if (flower == null)
            {
                return false;
            }

            _mapper.Map(model, flower);
            await _flowerRepository.UpdateAsync(flower);
            await _flowerRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var flower = await _flowerRepository.GetByIdAsync(id);

            if (flower == null)
            {
                return false;
            }

            await _flowerRepository.DeleteAsync(id);
            try
            {
                await _flowerRepository.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    "This flower cannot be deleted because it is connected to an existing order. Set its stock to 0 instead.",
                    ex);
            }

            return true;
        }
    }
}
