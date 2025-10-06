using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models.DTOs
{
    public class SupplierDTO
    {

            [Column(TypeName = "varchar(50)")]
            [Required]
            public string SupplierId { get; set; } = string.Empty;

            [Required]
            [MaxLength(250)]
            public string SupplierName { get; set; } = string.Empty;

            [MaxLength(250)]
            public string? RepresentativeName { get; set; }

            [MaxLength(50)]
            public string? RepresentativePhone { get; set; }

            [Required]
            public string Address { get; set; } = string.Empty;

            [Required]
            [MaxLength(50)]
            public string PhoneNo { get; set; } = string.Empty;

            [Required]
            [MaxLength(250)]
            public string Email { get; set; } = string.Empty;

            public string BranchId { get; set; } = string.Empty;

            public string CompanyId { get; set; } = string.Empty;


    }
}
