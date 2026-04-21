using GravyFoodsApi.Models.Accounting;

namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class JournalInfoDto
    {
        public string CompanyId { get; set; }
        public string BranchId { get; set; }

        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public string SourceModule { get; set; }
        public string Description { get; set; }

        public List<JournalDetailsDto> Details { get; set; } = new();
    }
}
