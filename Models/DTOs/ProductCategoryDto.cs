using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models.DTOs
{
    
    public class ProductCategoryDto
    {
        
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
        public int? ParentId { get; set; }

    }


    public class CategoryTreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Depth { get; set; }
        public int? ParentId { get; set; }
        public List<CategoryTreeDto> Children { get; set; } = new();

        

    }

}
