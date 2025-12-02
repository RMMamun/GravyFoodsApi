using GravyFoodsApi.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class PurchaseDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoSL { get; set; }

        [Required]
        [StringLength(450)]
        public string PurchaseId { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        public double Quantity { get; set; }

        [Required]
        [StringLength(20)]
        public string UnitType { get; set; } = string.Empty;

        public required string UnitId { get; set; } = string.Empty;
        public required string WHId { get; set; } = string.Empty;

        [Required]
        public double PricePerUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VATPerUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalVAT { get; set; }

        [StringLength(255)]
        public string? BranchId { get; set; }

        [StringLength(255)]
        public string? CompanyId { get; set; }

        // 🔗 Navigation properties
        [ForeignKey(nameof(PurchaseId))]
        public virtual PurchaseInfo? PurchaseInfo { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }
    }
}
