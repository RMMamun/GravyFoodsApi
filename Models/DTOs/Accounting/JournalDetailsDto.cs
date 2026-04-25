using GravyFoodsApi.Models.Accounting;

namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class JournalDetailsDto
    {
        public string ACCode { get; set; }
        public string? Description { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
