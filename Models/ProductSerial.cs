using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class ProductSerial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SerialNumber { get; set; } = string.Empty;

        
        [MaxLength(50)]
        public string? SKU { get; set; } = string.Empty;

        
        [MaxLength(50)]
        public string? IMEI1 { get; set; } = string.Empty;


        [MaxLength(50)]
        public string? IMEI2 { get; set; } = string.Empty;

        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string? PurchaseId { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public string? SalesId { get; set; }
        public DateTime? SalesDate { get; set; }

        public string? PurchaseReturnId { get; set; }
        public DateTime? PurchaseReturnDate { get; set; }

        public string? SalesReturnId { get; set; }
        public DateTime? SalesReturnDate { get; set; }

        [Required]
        public string WHId { get; set; } = string.Empty;

        [Required]
        public string StockHistory { get; set; } = string.Empty;

        [Required]
        public string StockStatus { get; set; } = string.Empty;

        [Required]
        public string BranchId { get; set; } = string.Empty;
        
        [Required]
        public string CompanyId { get; set; } = string.Empty;

        // Navigation
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;


    }
}
