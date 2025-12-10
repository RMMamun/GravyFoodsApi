using GravyFoodsApi.Models.DTOs;
using static GravyFoodsApi.MasjidServices.UnitConversionService;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IUnitConversionRepository
    {
        Task<double> ToBase(double value, int fromIndex, double[] segments);
        Task<double> FromBase(double baseValue, int toIndex, double[] segments);
        Task<double> Convert(double value, string fromUnit, string toUnit, string[] units, double[] segments);

        Task<Qty_UnitDto> ConvertToSmallUnit(double Quantity, string Unit, string UnitId, string BranchId, string CompanyId);
        Task<Qty_UnitDto> ConvertToShowingUnit(string ProductId, double Quantity, string Unit, string BranchId, string CompanyId);

        //Task<double> UnitConvert(double Quantity, string Unit, string unitId);
    }
}
