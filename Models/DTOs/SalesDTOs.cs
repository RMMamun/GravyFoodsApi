namespace GravyFoodsApi.Models.DTOs
{
    public class SalesInfoDto
    {
        public string SalesId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public List<SalesDetailDto> SalesDetails { get; set; } = new();
    }

    public class SalesDetailDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal DiscountPerUnit { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal VATPerUnit { get; set; }
        public decimal TotalVAT { get; set; }

        public string DiscountType { get; set; } = "%";

    }
}
