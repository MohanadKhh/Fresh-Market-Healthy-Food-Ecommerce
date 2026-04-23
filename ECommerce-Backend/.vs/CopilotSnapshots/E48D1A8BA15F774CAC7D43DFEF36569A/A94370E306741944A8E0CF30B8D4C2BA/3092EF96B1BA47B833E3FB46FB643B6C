using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" },
                new Category { Id = 3, Name = "Books" },
                new Category { Id = 4, Name = "Home Appliances" },
                new Category { Id = 5, Name = "Sports" }
            );

            modelBuilder.Entity<Product>().HasData(

                // Electronics
                new Product { Id = 1, Name = "Laptop", Description = "Gaming laptop", Price = 25000, Count = 5, CategoryId = 1, ExpiryDate = new DateOnly(2028, 1, 1) },
                new Product { Id = 2, Name = "Smartphone", Description = "Android phone", Price = 12000, Count = 10, CategoryId = 1, ExpiryDate = new DateOnly(2027, 6, 1) },
                new Product { Id = 3, Name = "Headphones", Description = "Wireless headphones", Price = 1500, Count = 15, CategoryId = 1, ExpiryDate = new DateOnly(2027, 3, 1) },
                new Product { Id = 4, Name = "Smart Watch", Description = "Fitness watch", Price = 3000, Count = 7, CategoryId = 1, ExpiryDate = new DateOnly(2027, 8, 1) },

                // Clothing
                new Product { Id = 5, Name = "T-Shirt", Description = "Cotton t-shirt", Price = 300, Count = 20, CategoryId = 2, ExpiryDate = new DateOnly(2026, 12, 1) },
                new Product { Id = 6, Name = "Jeans", Description = "Blue jeans", Price = 800, Count = 12, CategoryId = 2, ExpiryDate = new DateOnly(2027, 5, 1) },
                new Product { Id = 7, Name = "Jacket", Description = "Winter jacket", Price = 1500, Count = 6, CategoryId = 2, ExpiryDate = new DateOnly(2027, 11, 1) },
                new Product { Id = 8, Name = "Sneakers", Description = "Running shoes", Price = 1200, Count = 9, CategoryId = 2, ExpiryDate = new DateOnly(2027, 9, 1) },

                // Books
                new Product { Id = 9, Name = "C# Book", Description = "Learn C#", Price = 500, Count = 10, CategoryId = 3, ExpiryDate = new DateOnly(2029, 1, 1) },
                new Product { Id = 10, Name = "Java Book", Description = "Spring Boot guide", Price = 600, Count = 8, CategoryId = 3, ExpiryDate = new DateOnly(2029, 6, 1) },
                new Product { Id = 11, Name = "Data Science Book", Description = "Machine Learning basics", Price = 750, Count = 5, CategoryId = 3, ExpiryDate = new DateOnly(2029, 3, 1) },
                new Product { Id = 12, Name = "Algorithms Book", Description = "DSA concepts", Price = 650, Count = 4, CategoryId = 3, ExpiryDate = new DateOnly(2029, 5, 1) },

                // Home
                new Product { Id = 13, Name = "Microwave", Description = "800W microwave", Price = 4000, Count = 3, CategoryId = 4, ExpiryDate = new DateOnly(2028, 2, 1) },
                new Product { Id = 14, Name = "Refrigerator", Description = "Double door fridge", Price = 15000, Count = 2, CategoryId = 4, ExpiryDate = new DateOnly(2028, 7, 1) },
                new Product { Id = 15, Name = "Washing Machine", Description = "Automatic washer", Price = 10000, Count = 4, CategoryId = 4, ExpiryDate = new DateOnly(2028, 4, 1) },
                new Product { Id = 16, Name = "Air Conditioner", Description = "1.5 HP AC", Price = 18000, Count = 3, CategoryId = 4, ExpiryDate = new DateOnly(2028, 9, 1) },

                // Sports
                new Product { Id = 17, Name = "Football", Description = "FIFA standard ball", Price = 400, Count = 14, CategoryId = 5, ExpiryDate = new DateOnly(2027, 2, 1) },
                new Product { Id = 18, Name = "Tennis Racket", Description = "Professional racket", Price = 2000, Count = 5, CategoryId = 5, ExpiryDate = new DateOnly(2027, 10, 1) },
                new Product { Id = 19, Name = "Padel Racket", Description = "Carbon fiber padel", Price = 2500, Count = 6, CategoryId = 5, ExpiryDate = new DateOnly(2027, 12, 1) },
                new Product { Id = 20, Name = "Dumbbells", Description = "10kg pair", Price = 900, Count = 8, CategoryId = 5, ExpiryDate = new DateOnly(2027, 4, 1) }
            );

            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}
