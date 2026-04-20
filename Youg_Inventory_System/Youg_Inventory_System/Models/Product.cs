using System.ComponentModel.DataAnnotations;

namespace Youg_Inventory_System.Data
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Size { get; set; } = string.Empty;

        [Required]
        public string Color { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Range(0, int.MaxValue)]
        public int LowStockLevel { get; set; } = 10;

        public string? ImageData { get; set; }
    }
}
