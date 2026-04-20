using System.ComponentModel.DataAnnotations;

namespace Youg_Inventory_System.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string OrderId { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty; 

        [Required]
        public string Phone1 { get; set; } = string.Empty; 

        public string? Phone2 { get; set; } 

        [Required]
        public string Item { get; set; } = string.Empty; 

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Required]
        public string DeliveryType { get; set; } = "COD";

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; } = "Processing";
    }
}