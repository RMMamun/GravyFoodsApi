using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravyFoodsApi.Models.DTOs
{

    public class ProductSerialDto
    {

        
        public string ProductId { get; set; } = string.Empty;

        public string SerialNumber { get; set; } = string.Empty;

        public string? SKU { get; set; } = string.Empty;

        public string? IMEI1 { get; set; } = string.Empty;
        
        public string? IMEI2 { get; set; } = string.Empty;

        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

        public string? StockStatus { get; set; }

        public string? PurchaseId { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public string? SalesId { get; set; }
        public DateTime? SalesDate { get; set; }

        public string? PurchaseReturnId { get; set; }
        public DateTime? PurchaseReturnDate { get; set; }

        public string? SalesReturnId { get; set; }
        public DateTime? SalesReturnDate { get; set; }

        public string? WHId { get; set; }

        public string? StockHistory { get; set; }



    }


    public class ProductSerialDtoApiResp
    {

        [Required]
        [MaxLength(200)]
        public string ProductId { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        [Required]
        public string SerialNumber { get; set; } = string.Empty;

        public string? SKU { get; set; } = string.Empty;

        public string? IMEI1 { get; set; } = string.Empty;

        public string? IMEI2 { get; set; } = string.Empty;

        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;


    }


    public class PurchaseSerialsDto
    {

        [Required]
        public string PurchaseId { get; set; } = string.Empty;

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        public string SerialNumber { get; set; } = string.Empty;

        public string? SKU { get; set; } = string.Empty;

        public string? IMEI1 { get; set; } = string.Empty;

        public string? IMEI2 { get; set; } = string.Empty;

        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [Required]
        public string WHId { get; set; } = string.Empty;
        public string? StockHistory { get; set; }

        [Required]
        public string BranchId { get; set; } = string.Empty;

        [Required]
        public string CompanyId { get; set; } = string.Empty;


    }


    public class SalesSerialsDto
    {

        [Required]
        public string SsalesId { get; set; } = string.Empty;

        [Required]
        public DateTime SalesDate { get; set; }

        [Required]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        public string SerialNumber { get; set; } = string.Empty;

        public string? SKU { get; set; } = string.Empty;

        public string? IMEI1 { get; set; } = string.Empty;

        public string? IMEI2 { get; set; } = string.Empty;

        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string? WHId { get; set; }
        public string? StockHistory { get; set; }

        [Required]
        public string BranchId { get; set; } = string.Empty;

        [Required]
        public string CompanyId { get; set; } = string.Empty;


    }

}
