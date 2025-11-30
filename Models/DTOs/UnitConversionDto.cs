namespace GravyFoodsApi.Models.DTOs
{

    public class UnitConversionDto
    {
        public double Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;

        public string UnitId { get; set; } = string.Empty;

        public string CompanyId { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
    }

    //public class UnitConversionParameterDto 
    //{
    //    public double Quantity { get; set; }
    //    public string Unit { get; set; } = string.Empty;

    //    public string UnitId { get; set; } = string.Empty;

    //    public string[] UnitSegments { get; set; } = Array.Empty<string>();
    //    public double[] ValueSegments { get; set; } = Array.Empty<double>();

    //    public string CompanyId { get; set; } = string.Empty;
    //    public string BranchId { get; set; } = string.Empty;
    //}


}
