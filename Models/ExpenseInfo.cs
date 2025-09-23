using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class ExpenseInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ExpenseHeadId { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; } = DateTime.Now;

        public DateTime EntryDate { get; set; } = DateTime.Now;


        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        //public string? AttachmentPath { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public DateTime UpdatedAt { get; set; } = DateTime.Now;
        //public bool IsDeleted { get; set; } = false;


        // Navigation Properties
        [ForeignKey(nameof(ExpenseHeadId))]
        public ExpenseHead? ExpenseHead { get; set; }

        [ForeignKey(nameof(BranchId))]
        public BranchInfo BranchInfo { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public CompanyInfo CompanyInfo { get; set; }



    }
}
