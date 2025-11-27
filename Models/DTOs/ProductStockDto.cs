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

        public required string SmallUnit { get; set; }


    }
}
