using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class SalesOrderInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesId { get; set; }

        public string OrderStatus { get; set; } = "Pending"; // Pending, Completed, Cancelled, etc.
        public bool IsActive { get; set; } = true; // Default to true, can be set to false if order is cancelled or completed

        public string CustomerId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; } // Nullable in case not yet delivered
        public string? ShippingAddress { get; set; } // Nullable in case not applicable
        public string? BillingAddress { get; set; } // Nullable in case not applicable
        public string? PaymentMethod { get; set; } // Nullable in case not applicable
        public string? TransactionId { get; set; } // Nullable in case not applicable
        public DateTime? CreatedDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDateTime { get; set; } // Nullable in case not yet modified
        

        // Navigation


        [ForeignKey(nameof(CustomerId))]
        public CustomerInfo CustomerInfo { get; set; } = null!;
    }
}
