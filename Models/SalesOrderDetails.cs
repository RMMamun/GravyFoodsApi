using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class SalesOrderDetails
    {
        public int SalesId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        public double Quantity { get; set; }

        public double PricePerUnit { get; set; }
        public double Discount { get; set; } = 0;
        public string DiscountType { get; set; } = "%"; // Default to Percentage and Amount can be another option


        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
