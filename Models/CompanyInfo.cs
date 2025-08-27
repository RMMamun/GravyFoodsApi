using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class CompanyInfo
    {
        //public int Id { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // ProductId is PK, not Id
        public string CompanyId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Navigation Property (One-to-Many: Company -> Branches)
        public ICollection<BranchInfo> Branches { get; set; } = new List<BranchInfo>();
    }

}
