

using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.Models.DTOs
{

    public class SalesPaymentParamDto
    {
        public int TotalItems { get; set; }

        public decimal GrossAmount { get; set; }

        public decimal ItemWiseDiscount { get; set; }

        public decimal OverallDiscount { get; set; }

        public decimal DiscountOnPoints { get; set; }

        public decimal NetAmount { get; set; }

        public decimal VatAmount { get; set; }

        public decimal PayableAmount { get; set; }

        // Calculated Payment Info
        public decimal TotalPaid { get; set; }

        public decimal DueAmount { get; set; }

        public DateTime PaymentDate { get; set; }

        // Payment Method List
        public List<PaymentMethodsDto> PaymentMethods { get; set; } = new();
    }
        

}
