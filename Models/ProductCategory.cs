using Microsoft.Data.SqlClient;
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

        public bool IsActive { get; set; } = true;

        public int SortOrder { get; set; } = 0;

        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

        public int? ParentId { get; set; }
        public ProductCategory ParentCategory { get; set; }

        public ICollection<ProductCategory> Children { get; set; } = new List<ProductCategory>();

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
