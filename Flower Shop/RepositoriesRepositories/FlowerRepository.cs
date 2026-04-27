using FlowerShopOnlineOrderSystem.Data;
using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopOnlineOrderSystem.Repositories
{
    public class FlowerRepository : IFlowerRepository
    {
        private readonly ApplicationDbContext _context;

        public FlowerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flower>> GetAllAsync()
        {
            return await _context.Flowers.ToListAsync();
        }

        public async Task<Flower?> GetByIdAsync(int id)
        {
            return await _context.Flowers.FindAsync(id);
        }

        public async Task AddAsync(Flower flower)
        {
            await _context.Flowers.AddAsync(flower);
        }

        public Task UpdateAsync(Flower flower)
        {
            _context.Flowers.Update(flower);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var flower = await GetByIdAsync(id);

            if (flower != null)
            {
                _context.Flowers.Remove(flower);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}