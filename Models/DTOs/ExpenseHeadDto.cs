using System.ComponentModel.DataAnnotations;

namespace GravyFoodsApi.Models.DTOs
{
    public class ExpenseHeadDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string HeadName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AccountCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //public string BranchId { get; set; } = string.Empty;
        //public string CompanyId { get; set; } = string.Empty;




    }
}
