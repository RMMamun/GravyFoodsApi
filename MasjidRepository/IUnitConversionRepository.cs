namespace GravyFoodsApi.MasjidRepository
{
    public interface IUnitConversionRepository
    {
        Task<double> ToBase(double value, int fromIndex, double[] segments);
        double FromBase(double baseValue, int toIndex, double[] segments);
        Task<double> Convert(double value, string fromUnit, string toUnit, string[] units, double[] segments);
        Task<double> UnitConvert(double Quantity, string Unit, string unitId);
    }
}
