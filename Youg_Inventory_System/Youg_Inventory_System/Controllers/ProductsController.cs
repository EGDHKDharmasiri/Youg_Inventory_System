using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Youg_Inventory_System.Data;

namespace Youg_Inventory_System.Controllers
{
    /// <summary>
    /// API controller for managing products in the inventory system.
    /// Provides endpoints for CRUD operations on products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ProductsController> _logger;

        /// <summary>
        /// Initializes a new instance of the ProductsController class.
        /// </summary>
        /// <param name="dbContext">The application database context for data access.</param>
        /// <param name="logger">The logger for logging controller operations.</param>
        public ProductsController(AppDbContext dbContext, ILogger<ProductsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>A list of all products. Returns 200 OK on success, 500 on error.</returns>
        /// <response code="200">Returns the list of products successfully.</response>
        /// <response code="500">If there's an error retrieving products from the database.</response>
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts called - retrieving all products from database");
                var products = await _dbContext.Products.ToListAsync();
                _logger.LogInformation("Successfully retrieved {ProductCount} products", products.Count);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products from database");
                return StatusCode(500, new
                {
                    message = "Error retrieving products",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves a specific product by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>The product if found. Returns 200 OK on success, 404 if not found.</returns>
        /// <response code="200">Returns the product successfully.</response>
        /// <response code="404">If the product with the specified ID is not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                _logger.LogInformation("GetProductById called with id: {ProductId}", id);
                var product = await _dbContext.Products.FindAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with id {ProductId} not found", id);
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with id: {ProductId}", id);
                return StatusCode(500, new
                {
                    message = "Error retrieving product",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Creates a new product in the database.
        /// Validates that the product code is unique before creation.
        /// </summary>
        /// <param name="product">The product object to create.</param>
        /// <returns>Returns the created product with 201 Created status if successful.</returns>
        /// <response code="201">Returns the newly created product.</response>
        /// <response code="400">If the product code already exists or validation fails.</response>
        /// <response code="500">If there's an error saving to the database.</response>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            try
            {
                // Validate input
                if (product == null)
                {
                    _logger.LogWarning("CreateProduct called with null product");
                    return BadRequest(new { message = "Product cannot be null." });
                }

                // Check for required fields
                if (string.IsNullOrWhiteSpace(product.ProductCode) ||
                    string.IsNullOrWhiteSpace(product.Name) ||
                    string.IsNullOrWhiteSpace(product.Size) ||
                    string.IsNullOrWhiteSpace(product.Color))
                {
                    _logger.LogWarning("CreateProduct called with missing required fields");
                    return BadRequest(new { message = "ProductCode, Name, Size, and Color are required fields." });
                }

                // Check if product code already exists
                var existingProduct = await _dbContext.Products
                    .AnyAsync(p => p.ProductCode == product.ProductCode);

                if (existingProduct)
                {
                    _logger.LogWarning("CreateProduct called with duplicate ProductCode: {ProductCode}", product.ProductCode);
                    return Conflict(new { message = "Product Code already exists. Please use a unique product code." });
                }

                // Add the product to the database
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Product created successfully with id: {ProductId}", product.Id);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, new
                {
                    message = "Error creating product",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="product">The updated product object.</param>
        /// <returns>Returns 204 No Content on success, 400 if ID mismatch, 404 if not found.</returns>
        /// <response code="204">Product updated successfully.</response>
        /// <response code="400">If the product ID in the URL doesn't match the product object.</response>
        /// <response code="404">If the product with the specified ID is not found.</response>
        /// <response code="409">If trying to change ProductCode to one that already exists.</response>
        /// <response code="500">If there's an error saving to the database.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogWarning("UpdateProduct called with null product");
                    return BadRequest(new { message = "Product cannot be null." });
                }

                if (id != product.Id)
                {
                    _logger.LogWarning("UpdateProduct called with mismatched IDs: URL={UrlId}, Body={BodyId}", id, product.Id);
                    return BadRequest(new { message = "Product ID in URL must match the ID in the request body." });
                }

                var existingProduct = await _dbContext.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("UpdateProduct called for non-existent product with id: {ProductId}", id);
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }

                // Check if product code is being changed to a duplicate
                if (existingProduct.ProductCode != product.ProductCode)
                {
                    var isDuplicate = await _dbContext.Products
                        .AnyAsync(p => p.ProductCode == product.ProductCode && p.Id != id);

                    if (isDuplicate)
                    {
                        _logger.LogWarning("UpdateProduct attempted with duplicate ProductCode: {ProductCode}", product.ProductCode);
                        return Conflict(new { message = "Product Code already exists. Please use a unique product code." });
                    }
                }

                // Update the product properties
                existingProduct.ProductCode = product.ProductCode;
                existingProduct.Name = product.Name;
                existingProduct.Size = product.Size;
                existingProduct.Color = product.Color;
                existingProduct.Price = product.Price;
                existingProduct.StockQuantity = product.StockQuantity;
                existingProduct.LowStockLevel = product.LowStockLevel;

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Product with id {ProductId} updated successfully", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with id: {ProductId}", id);
                return StatusCode(500, new
                {
                    message = "Error updating product",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Deletes a product from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>Returns 204 No Content on success, 404 if not found.</returns>
        /// <response code="204">Product deleted successfully.</response>
        /// <response code="404">If the product with the specified ID is not found.</response>
        /// <response code="500">If there's an error deleting from the database.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("DeleteProduct called for non-existent product with id: {ProductId}", id);
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Product with id {ProductId} deleted successfully", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with id: {ProductId}", id);
                return StatusCode(500, new
                {
                    message = "Error deleting product",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Searches for products by a partial product code or name match.
        /// </summary>
        /// <param name="searchTerm">The search term to look for in ProductCode or Name.</param>
        /// <returns>A list of products matching the search term.</returns>
        /// <response code="200">Returns matching products.</response>
        /// <response code="500">If there's an error searching the database.</response>
        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<List<Product>>> SearchProducts(string searchTerm)
        {
            try
            {
                _logger.LogInformation("SearchProducts called with searchTerm: {SearchTerm}", searchTerm);

                var products = await _dbContext.Products
                    .Where(p => p.ProductCode.Contains(searchTerm) || p.Name.Contains(searchTerm))
                    .ToListAsync();

                _logger.LogInformation("Search returned {ProductCount} results", products.Count);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching products with term: {SearchTerm}", searchTerm);
                return StatusCode(500, new
                {
                    message = "Error searching products",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Gets products with low stock (stock quantity below low stock level).
        /// </summary>
        /// <returns>A list of products with low stock.</returns>
        /// <response code="200">Returns products with low stock.</response>
        /// <response code="500">If there's an error retrieving data.</response>
        [HttpGet("low-stock")]
        public async Task<ActionResult<List<Product>>> GetLowStockProducts()
        {
            try
            {
                _logger.LogInformation("GetLowStockProducts called");
                var lowStockProducts = await _dbContext.Products
                    .Where(p => p.StockQuantity < p.LowStockLevel)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {ProductCount} products with low stock", lowStockProducts.Count);
                return Ok(lowStockProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving low stock products");
                return StatusCode(500, new
                {
                    message = "Error retrieving low stock products",
                    error = ex.Message
                });
            }
        }
    }
}
