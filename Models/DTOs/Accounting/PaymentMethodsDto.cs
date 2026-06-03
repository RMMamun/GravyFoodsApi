namespace GravyFoodsApi.Models.DTOs.Accounting
{
    //public class PaymentMethodsDto
    //{
    //    public Guid MethodId { get; set; }
    //    public string PaymentMethodName { get; set; }
    //    public string PaymentMethodCode { get; set; }
    //    public Guid AccountId { get; set; }

    //    public int SequenceNo { get; set; }

    //    public bool SystemData { get; set; }

    //}

    public class PaymentMethodsDto
    {
        public Guid MethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodCode { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; } = 0;

        public int SequenceNo { get; set; }

        public bool SystemData { get; set; }
    }

}
