namespace GravyFoodsApi.Models.DTOs
{
    public class PurchaseInfoDto
    {
        public string PurchaseId { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

        public double TotalAmount { get; set; }
        public double? PaidAmount { get; set; }
        public double? DueAmount { get; set; }


        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public List<PurchaseDetailDto> PurchaseDetails { get; set; } = new();
    }

    public class PurchaseDetailDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public string UnitType { get; set; }

        public string UnitId { get; set; }
        public string WHId { get; set; }

        public double PricePerUnit { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal VATPerUnit { get; set; }
        public decimal TotalVAT { get; set; }

        public bool IsSerialBased { get; set; } = false;

        public string UserId { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;


        public List<ProductSerialDto>? ProductSerial { get; set; } = new();

    }
}
