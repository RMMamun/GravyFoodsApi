using GravyFoodsApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class PurchaseInfo
    {
        [Key]
        [Required]
        [StringLength(450)]
        public string PurchaseId { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoSL { get; set; }

        [Required]
        [StringLength(50)]
        public string SupplierId { get; set; } = string.Empty;

        public DateTime? PurchaseDate { get; set; }

        [StringLength(100)]
        public string? PaymentMethod { get; set; }

        [StringLength(250)]
        public string? TransactionId { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        public DateTime? EditDateTime { get; set; }

        [Required]
        public double TotalAmount { get; set; }
        public double? PaidAmount { get; set; }
        public double? DueAmount { get; set; }

        [StringLength(255)]
        public string? BranchId { get; set; }

        [StringLength(255)]
        public string? CompanyId { get; set; }

        [StringLength(50)]
        public string? UserId { get; set; }

        // 🔗 Navigation properties
        [ForeignKey(nameof(SupplierId))]
        public virtual SupplierInfo? SupplierInfo { get; set; }

        public virtual ICollection<PurchaseDetails>? PurchaseDetails { get; set; }
    }
}
