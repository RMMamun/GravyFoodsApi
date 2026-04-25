namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class TrialBalanceDto
    {
        public string ACCode { get; set; }
        public string AccountName { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
