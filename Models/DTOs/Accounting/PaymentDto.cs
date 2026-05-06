namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class PaymentDto
    {
        public Guid MethodId { get; set; } // Cash / Bank / bKash
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }


}
