using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class Warehouse
    {
        [Key]
        [Required]
        [StringLength(200)]
        public string WHId { get; set; } = string.Empty;   // Primary Key

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }

        [Required]
        [StringLength(300)]
        public string WHName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? WHLocationNo { get; set; }

        [Required]
        [StringLength(255)]
        public string BranchId { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string CompanyId { get; set; } = string.Empty;
    }
}
