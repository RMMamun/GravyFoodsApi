namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class PaymentMethodsDto
    {
        public Guid MethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodCode { get; set; }
        public Guid AccountId { get; set; }
    }
}
