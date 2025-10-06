using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class SalesInfo
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoSL { get; set; }

        [Key]
        public required string SalesId { get; set; }

        public string OrderStatus { get; set; } = "Pending"; // Pending, Completed, Cancelled, etc.

        public required string CustomerId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; } // Nullable in case not yet delivered
        public string? ShippingAddress { get; set; } // Nullable in case not applicable
        public string? BillingAddress { get; set; } // Nullable in case not applicable
        public string? PaymentMethod { get; set; } // Nullable in case not applicable
        public string? TransactionId { get; set; } // Nullable in case not applicable
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; } // Nullable in case not yet modified

        public double TotalAmount { get; set; }
        public double TotalDiscountAmount { get; set; }
        public double TotalPaidAmount { get; set; }

        public string UserId { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        
        
        // Navigation Properties
        [ForeignKey(nameof(BranchId))]
        public BranchInfo BranchInfo { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public CompanyInfo CompanyInfo { get; set; }
        

        [ForeignKey(nameof(CustomerId))]
        public CustomerInfo CustomerInfo { get; set; } = null!;

        public ICollection<SalesDetails> SalesDetails { get; set; } = new List<SalesDetails>();
    }
}
