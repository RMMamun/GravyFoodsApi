using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models.DTOs
{
    public class ProductStockDto
    {

        [Required]
        [MaxLength(200)]
        public string ProductId { get; set; } = string.Empty;

        [MaxLength(200)]
        public string WHId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string SmallUnit { get; set; }

        [Required]
        [StringLength(255)]
        public string BranchId { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string CompanyId { get; set; } = string.Empty;


    }

    public class LowStockProductsDto
    {

        [Required]
        [MaxLength(200)]
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string WHId { get; set; }
        public string WHName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string ShowingUnit { get; set; }

        [Required]
        [StringLength(255)]
        public string BranchId { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string CompanyId { get; set; } = string.Empty;


    }


    public class ProductsStockDto
    {

        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;

        public string WHId { get; set; }
        public string WHName { get; set; }

        public int Quantity { get; set; }

        public string ShowingUnit { get; set; }

        public string BranchName { get; set; } = string.Empty;



    }



}
