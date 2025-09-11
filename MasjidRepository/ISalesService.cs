using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ISalesService
    {
        Task<IEnumerable<SalesInfo>> GetAllSalesAsync();
        Task<SalesInfo?> GetSaleByIdAsync(string salesId);
        Task<SalesInfoDto> CreateSaleAsync(SalesInfoDto sale);
        Task<SalesInfo?> UpdateSaleAsync(string salesId, SalesInfo sale);
        Task<bool> DeleteSaleAsync(string salesId);
    }
}
