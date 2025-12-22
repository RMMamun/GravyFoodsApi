namespace GravyFoodsApi.Models.DTOs
{
    public class BestSoldProductsDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public decimal TotalQuantitySold { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalVAT { get; set; }


        public string BranchId { get; set; } = string.Empty;

        public string CompanyId { get; set; } = string.Empty;


    }
}
