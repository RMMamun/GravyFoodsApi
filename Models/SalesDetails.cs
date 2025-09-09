using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class SalesDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoSL { get; set; }


        [Required]
        [MaxLength(200)]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        public double Quantity { get; set; }
        public double UnitType { get; set; }    //PCS,KG,Ltr
        public double PricePerUnit { get; set; }
        public double DiscountPerUnit { get; set; } = 0;
        public double DiscountAmountPerUnit { get; set; } = 0;
        public string DiscountType { get; set; } = "%"; // Default to Percentage and Amount can be another option


        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(SalesId))]

        public required string SalesId { get; set; }

        public SalesInfo SalesInfo { get; set; } = null!;

    }
}
