using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class ProductStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductId { get; set; } = string.Empty;

        [MaxLength(200)]
        public string WHId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string SmallUnit { get; set; }

        [Required]
        [StringLength(255)]
        public string BranchId { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string CompanyId { get; set; } = string.Empty;

        // Navigation
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }

}
