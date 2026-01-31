using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? CountryOfOrigin { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; }

        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }


    public class BrandDto
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? CountryOfOrigin { get; set; }

        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; }


    }


}
