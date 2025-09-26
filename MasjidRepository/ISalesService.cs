using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ISalesService
    {
        Task<IEnumerable<SalesInfoDto>> GetAllSalesAsync();
        Task<SalesInfo?> GetSaleByIdAsync(string salesId);
        Task<SalesInfoDto> CreateSaleAsync(SalesInfoDto sale);
        Task<SalesInfo?> UpdateSaleAsync(string salesId, SalesInfo sale);
        Task<bool> DeleteSaleAsync(string salesId);

        Task<IEnumerable<SalesInfoDto>> GetSalesByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId);

    }
}
