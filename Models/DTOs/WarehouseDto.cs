namespace GravyFoodsApi.DTOs
{
    public class WarehouseDto
    {
        public string WHId { get; set; } = string.Empty;

        public string WHName { get; set; }

        public string? WHLocationNo { get; set; }

        public string BranchId { get; set; } = string.Empty;

        public string CompanyId { get; set; } = string.Empty;
    }
}
