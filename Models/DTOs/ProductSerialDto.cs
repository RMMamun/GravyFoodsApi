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
}
