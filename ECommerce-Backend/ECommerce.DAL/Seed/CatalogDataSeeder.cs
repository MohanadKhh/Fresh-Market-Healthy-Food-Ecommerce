using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.DAL
{
    public static class CatalogDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            if (await dbContext.Categories.AnyAsync() || await dbContext.Products.AnyAsync())
                return;

            var categories = new List<Category>
            {
                new() { Name = "Fresh Fruits", Description = "Seasonal fresh fruits full of natural sweetness and vitamins", ImageUrl = null, IsDeleted = false },
                new() { Name = "Vegetables", Description = "Farm-fresh vegetables for healthy everyday meals", ImageUrl = null, IsDeleted = false },
                new() { Name = "Herbs & Greens", Description = "Fresh herbs and leafy greens", ImageUrl = null, IsDeleted = false },
                new() { Name = "Dried Fruits", Description = "Premium dried fruits", ImageUrl = null, IsDeleted = false },
                new() { Name = "Nuts", Description = "Healthy nuts and seeds", ImageUrl = null, IsDeleted = false }
            };

            var products = new List<Product>
            {
                // Fresh Fruits (CategoryId = 1)
                new() { Name = "Red Apple", Description = "Fresh sweet apples", Price = 70, Stock = 150, Unit = "kg", Reviews = 120, IsOrganic = true, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 1), ExpiryDate = new DateOnly(2026, 5, 10), ImageUrl = null, CategoryId = 1, IsDeleted = false },
                new() { Name = "Banana", Description = "Ripe bananas", Price = 40, Stock = 200, Unit = "kg", Reviews = 89, IsOrganic = false, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 2), ExpiryDate = new DateOnly(2026, 5, 8), ImageUrl = null, CategoryId = 1, IsDeleted = false },
                new() { Name = "Strawberries", Description = "Sweet strawberries", Price = 120, Stock = 90, Unit = "kg", Reviews = 95, IsOrganic = true, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 3), ExpiryDate = new DateOnly(2026, 5, 7), ImageUrl = null, CategoryId = 1, IsDeleted = false },

                // Vegetables (CategoryId = 2)
                new() { Name = "Tomatoes", Description = "Fresh tomatoes", Price = 30, Stock = 140, Unit = "kg", Reviews = 110, IsOrganic = false, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 4), ExpiryDate = new DateOnly(2026, 5, 9), ImageUrl = null, CategoryId = 2, IsDeleted = false },
                new() { Name = "Carrots", Description = "Crunchy carrots", Price = 20, Stock = 180, Unit = "kg", Reviews = 75, IsOrganic = true, IsFeatured = false, CreatedAt = new DateOnly(2026, 4, 5), ExpiryDate = new DateOnly(2026, 5, 15), ImageUrl = null, CategoryId = 2, IsDeleted = false },
                new() { Name = "Cucumbers", Description = "Fresh cucumbers", Price = 25, Stock = 130, Unit = "kg", Reviews = 64, IsOrganic = false, IsFeatured = false, CreatedAt = new DateOnly(2026, 4, 6), ExpiryDate = new DateOnly(2026, 5, 11), ImageUrl = null, CategoryId = 2, IsDeleted = false },

                // Herbs & Greens (CategoryId = 3)
                new() { Name = "Spinach", Description = "Fresh spinach leaves", Price = 15, Stock = 120, Unit = "bunch", Reviews = 98, IsOrganic = true, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 7), ExpiryDate = new DateOnly(2026, 5, 5), ImageUrl = null, CategoryId = 3, IsDeleted = false },
                new() { Name = "Parsley", Description = "Fresh parsley", Price = 10, Stock = 90, Unit = "bunch", Reviews = 70, IsOrganic = false, IsFeatured = false, CreatedAt = new DateOnly(2026, 4, 8), ExpiryDate = new DateOnly(2026, 5, 6), ImageUrl = null, CategoryId = 3, IsDeleted = false },
                new() { Name = "Basil", Description = "Fresh basil", Price = 12, Stock = 100, Unit = "bunch", Reviews = 89, IsOrganic = true, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 9), ExpiryDate = new DateOnly(2026, 5, 6), ImageUrl = null, CategoryId = 3, IsDeleted = false },

                // Dried Fruits (CategoryId = 4)
                new() { Name = "Dates", Description = "Sweet dried dates", Price = 150, Stock = 60, Unit = "kg", Reviews = 234, IsOrganic = true, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 10), ExpiryDate = new DateOnly(2026, 10, 1), ImageUrl = null, CategoryId = 4, IsDeleted = false },
                new() { Name = "Raisins", Description = "Dried raisins", Price = 90, Stock = 120, Unit = "kg", Reviews = 90, IsOrganic = false, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 11), ExpiryDate = new DateOnly(2026, 9, 1), ImageUrl = null, CategoryId = 4, IsDeleted = false },
                new() { Name = "Dried Figs", Description = "Soft dried figs", Price = 130, Stock = 80, Unit = "kg", Reviews = 145, IsOrganic = true, IsFeatured = false, CreatedAt = new DateOnly(2026, 4, 12), ExpiryDate = new DateOnly(2026, 11, 1), ImageUrl = null, CategoryId = 4, IsDeleted = false },

                // Nuts (CategoryId = 5)
                new() { Name = "Almonds", Description = "Raw almonds", Price = 300, Stock = 115, Unit = "kg", Reviews = 267, IsOrganic = true, IsFeatured = false, CreatedAt = new DateOnly(2026, 4, 13), ExpiryDate = new DateOnly(2026, 12, 1), ImageUrl = null, CategoryId = 5, IsDeleted = false },
                new() { Name = "Cashews", Description = "Fresh cashews", Price = 350, Stock = 100, Unit = "kg", Reviews = 200, IsOrganic = false, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 14), ExpiryDate = new DateOnly(2026, 12, 1), ImageUrl = null, CategoryId = 5, IsDeleted = false },
                new() { Name = "Walnuts", Description = "Healthy walnuts", Price = 320, Stock = 80, Unit = "kg", Reviews = 150, IsOrganic = true, IsFeatured = true, CreatedAt = new DateOnly(2026, 4, 15), ExpiryDate = new DateOnly(2026, 12, 1), ImageUrl = null, CategoryId = 5, IsDeleted = false }
            };

            await dbContext.Categories.AddRangeAsync(categories);
            await dbContext.Products.AddRangeAsync(products);
            await dbContext.SaveChangesAsync();
        }
    }
}
