using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // ProductId is PK, not Id
        [MaxLength(200)]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ProductCode { get; set; } = string.Empty;
        public string? SKUCode { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int? BrandId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountedPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        [Required]
        public string UnitId { get; set; }

        [Required]
        public string DefaultUnit { get; set; } = string.Empty;


        [Required]
        public bool IsAvailable { get; set; } = true;

        [Required]
        public bool IsSalable { get; set; }

        [Required]
        public bool IsDigital { get; set; } = false;

        [MaxLength(500)]
        public string? DownloadUrl { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        public string? BranchId { get; set; }
        public string? CompanyId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(BranchId))]
        public BranchInfo? Branch { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(CompanyId))]
        public CompanyInfo? Company { get; set; }


        // Navigation Properties
        [ForeignKey(nameof(BrandId))]
        public Brand? Brand { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public ProductCategory Category { get; set; } = null!;

        public ProductUnits Unit { get; set; } = null!;

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
        public ICollection<ProductSerial> Serials { get; set; } = new List<ProductSerial>();
        public ICollection<ProductStock> Stocks { get; set; } = new List<ProductStock>();
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    }


    public class ProductDto
    {

        public string ProductId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ProductCode { get; set; } = string.Empty;
        public string? SKUCode { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int? BrandId { get; set; }
        public string BrandName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountedPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        //[Column(TypeName = "decimal(18,2)")]
        public double Quantity { get; set; }
        public string UnitId { get; set; }

        public string DefaultUnit { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        public bool IsSalable { get; set; }

        public string ImageUrl { get; set; }

        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }


}

