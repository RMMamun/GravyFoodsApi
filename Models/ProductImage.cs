using GravyFoodsApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(255)]
        public string BranchId { get; set; } = string.Empty;

        [MaxLength(255)]
        public string CompanyId { get; set; } = string.Empty;

        // Navigation
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }


    public class ProductImageDTO
    {
        public string ProductId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public byte[] ImageAsByte { get; set; } = Array.Empty<byte>();
        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

    }
}

    

  
