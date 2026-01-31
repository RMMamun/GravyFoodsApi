namespace GravyFoodsApi.Models.DTOs
{
    public class SalesInfoDto
    {
        public string SalesId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

        public string OrderStatus { get; set; } = "Pending";
        public double TotalAmount { get; set; }
        public double TotalDiscountAmount { get; set; }
        public double TotalPaidAmount { get; set; }

        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public List<SalesDetailDto> SalesDetails { get; set; } = new();
    }

    public class SalesDetailDto
    {
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

        public string DiscountType { get; set; } = "%";

        public string WHId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;


    }
}
