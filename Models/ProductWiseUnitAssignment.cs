using System.Globalization;

namespace GravyFoodsApi.Models
{
    public class ProductWiseUnitAssignment
    {
        public int Id { get; set; }
        public string UnitId { get; set; } = string.Empty;
        public string ProductId { get; set; }
        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

        // Navigation Properties
        public ProductUnits Unit { get; set; }
        public BranchInfo Branch { get; set; }
        public CompanyInfo Company { get; set; }
    }

}
