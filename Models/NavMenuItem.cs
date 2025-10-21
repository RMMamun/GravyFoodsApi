using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{

    public class NavMenuItem
    {

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public int Id { get; set; }

        [Key]
        public int MenuId { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public NavMenuItem? Parent { get; set; }

        public ICollection<NavMenuItem>? Children { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Url { get; set; }

        [MaxLength(100)]
        public string? IconCss { get; set; }

        [MaxLength(200)]
        public string? IconImagePath { get; set; }

        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsSeparator { get; set; } = false;

        public string BranchId { get; set; }
        public string CompanyId { get; set; }
    }


}
