namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class TrialBalanceDto
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
