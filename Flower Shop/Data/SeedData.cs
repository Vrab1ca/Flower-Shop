using FlowerShopOnlineOrderSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopOnlineOrderSystem.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            foreach (var role in new[] { RoleNames.Administrator, RoleNames.Customer, RoleNames.Florist })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            await CreateUserAsync(userManager, "admin@flowershop.local", "Admin123!", RoleNames.Administrator);
            await CreateUserAsync(userManager, "florist@flowershop.local", "Florist123!", RoleNames.Florist);
            await CreateUserAsync(userManager, "customer@flowershop.local", "Customer123!", RoleNames.Customer);

            if (!await context.Flowers.AnyAsync())
            {
                context.Flowers.AddRange(
                    new Flower
                    {
                        Name = "Rose Celebration Bouquet",
                        Description = "Classic red and blush roses arranged with seasonal greenery.",
                        Price = 49.99m,
                        AvailableQuantity = 25,
                        ImageUrl = "https://images.unsplash.com/photo-1518709779341-56cf4535e94b?auto=format&fit=crop&w=900&q=80"
                    },
                    new Flower
                    {
                        Name = "Spring Tulip Mix",
                        Description = "Bright tulips in a cheerful wrapped arrangement.",
                        Price = 34.50m,
                        AvailableQuantity = 40,
                        ImageUrl = "https://images.unsplash.com/photo-1490750967868-88aa4486c946?auto=format&fit=crop&w=900&q=80"
                    },
                    new Flower
                    {
                        Name = "Orchid Tablepiece",
                        Description = "Elegant orchid stems styled for gifts, desks, and reception spaces.",
                        Price = 59.00m,
                        AvailableQuantity = 12,
                        ImageUrl = "https://images.unsplash.com/photo-1526047932273-341f2a7631f9?auto=format&fit=crop&w=900&q=80"
                    });

                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateUserAsync(
            UserManager<IdentityUser> userManager,
            string email,
            string password,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Unable to create seed user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
