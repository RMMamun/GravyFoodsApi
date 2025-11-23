namespace GravyFoodsApi.Models.DTOs
{
    public class ProductUnitsDto
    {

        public string UnitId { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public string UnitDescription { get; set; } = string.Empty;
        public string UnitSegments { get; set; } = string.Empty;
        public string UnitSegmentsRatio { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

    }

}
