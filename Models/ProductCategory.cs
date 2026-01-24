using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
