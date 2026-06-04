using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.Models.DTOs
{
    public class SalesInfoDto
    {
        public string SalesId { get; set; } = string.Empty;

        public string InvoiceNo { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;

        public string OrderStatus { get; set; }
        public double TotalAmount { get; set; }
        public double TotalDiscountAmount { get; set; }

        public double TotalVATAmount { get; set; }
        public double TotalTaxAmount { get; set; }

        public double TotalPayableAmount { get; set; }
        public double TotalPaidAmount { get; set; }
        public double CashReceived { get; set; }
        public double DueAmount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public List<SalesDetailDto> SalesDetails { get; set; } = new();

        //public SalesPaymentParamDto SalesPaymentParam { get; set; } = new();
        public List<PaymentMethodsDto> PaymentDto { get; set; } = new();


    }

    public class SalesDetailDto
    {
        public string SalesId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public string UnitType { get; set; } = string.Empty;
        public string UnitId { get; set; } = string.Empty;
        public double PricePerUnit { get; set; }
        public double DiscountPerUnit { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal VATPerUnit { get; set; }
        public decimal TotalVAT { get; set; }
        public decimal TotalTax { get; set; }

        public string DiscountType { get; set; } = "%";

        public string WHId { get; set; } = string.Empty;

    }
}
