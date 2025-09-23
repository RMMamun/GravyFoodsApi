
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class ExpenseHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string HeadName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AccountCode { get; set; }

        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        
        //public DateTime UpdatedAt { get; set; } = DateTime.Now;


        // Navigation
        public ICollection<ExpenseInfo> ExpenseInfo { get; set; } = new List<ExpenseInfo>();

        //BranchId is the foreign key of BranchInfo model
        // Navigation Properties
        [ForeignKey(nameof(BranchId))]
        public BranchInfo BranchInfo { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public CompanyInfo CompanyInfo { get; set; }


    }
}
