using FlowerShopOnlineOrderSystem.Data;
using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Flower_Shop.Tests
{
    public class FlowerRepositoryTests
    {
        [Fact]
        public async Task GetAvailableAsync_ReturnsOnlyFlowersInStock()
        {
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            await using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var repository = new FlowerRepository(context);
            await repository.AddAsync(new Flower
            {
                Name = "Garden Roses",
                Description = "Soft pink roses",
                Price = 20m,
                AvailableQuantity = 5
            });
            await repository.AddAsync(new Flower
            {
                Name = "Sold Out Orchid",
                Description = "Orchid stems",
                Price = 30m,
                AvailableQuantity = 0
            });
            await repository.SaveChangesAsync();

            var available = await repository.GetAvailableAsync();

            var flower = Assert.Single(available);
            Assert.Equal("Garden Roses", flower.Name);
            Assert.Equal(5, flower.AvailableQuantity);
        }
    }
}
