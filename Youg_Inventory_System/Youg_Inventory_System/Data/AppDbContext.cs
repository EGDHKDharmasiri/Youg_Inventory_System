using Microsoft.EntityFrameworkCore;
using Youg_Inventory_System.Data;
using Youg_Inventory_System.Models;

namespace Youg_Inventory_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductCode)
                .IsUnique();

            // Optional: Set maximum length for ProductCode
            modelBuilder.Entity<Product>()
                .Property(p => p.ProductCode)
                .HasMaxLength(50)
                .IsRequired();

            // Seed default admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "password123",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed default product
            modelBuilder.Entity<Product>().HasData(
                new Product 
                { 
                    Id = 1, 
                    ProductCode = "ADMIN", 
                    Name = "System Admin Account", 
                    Size = "N/A", 
                    Color = "N/A", 
                    Price = 0, 
                    StockQuantity = 0, 
                    LowStockLevel = 0 
                }
            );
        }
    }
}


