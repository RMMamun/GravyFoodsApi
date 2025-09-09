using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ISalesService
    {
        Task<IEnumerable<SalesInfo>> GetAllSalesAsync();
        Task<SalesInfo?> GetSaleByIdAsync(string salesId);
        Task<SalesInfo> CreateSaleAsync(SalesInfo sale);
        Task<SalesInfo?> UpdateSaleAsync(string salesId, SalesInfo sale);
        Task<bool> DeleteSaleAsync(string salesId);
    }
}
