using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GravyFoodsApi.Models
{
    public class ProductUnits
    {
        //public int Id { get; set; }
        [Key]
        public string UnitId { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public string UnitDescription { get; set; } = string.Empty;
        public string UnitSegments { get; set; } = string.Empty;
        public string UnitSegmentsRatio { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;

        // Navigation Properties
        public BranchInfo Branch { get; set; }
        public CompanyInfo Company { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}
