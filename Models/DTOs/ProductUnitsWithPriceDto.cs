namespace GravyFoodsApi.Models.DTOs
{
    public class ProductUnitsWithPriceDto
    {
        public string UnitId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; } = 0;
    }
}
