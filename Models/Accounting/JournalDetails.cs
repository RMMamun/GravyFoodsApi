namespace GravyFoodsApi.Models.Accounting
{
    public class JournalDetails : BaseEntity
    {
        public Guid JournalId { get; set; }
        public Guid AccountId { get; set; }
        public string? Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
