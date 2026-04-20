using Microsoft.EntityFrameworkCore;
using Youg_Inventory_System.Data;
using Youg_Inventory_System.Data;

namespace Youg_Inventory_System.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<List<Product>> GetLowStockProductsAsync();
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Creates a new product in the database.
        /// </summary>
        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Deletes a product from the database by its ID.
        /// </summary>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all products with stock below their low stock level.
        /// </summary>
        public async Task<List<Product>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .Where(p => p.StockQuantity <= p.LowStockLevel)
                .ToListAsync();
        }
    }
}
