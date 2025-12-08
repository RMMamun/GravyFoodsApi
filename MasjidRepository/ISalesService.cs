using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ISalesService
    {

        Task<ApiResponse<SalesInfoDto>> CreateSalesAsync(SalesInfoDto sale);

        Task<IEnumerable<SalesInfoDto>> GetAllSalesAsync();
        Task<ApiResponse<SalesInfoDto>> GetSaleByIdAsync(string salesId, string branchId, string companyId);
        Task<SalesInfo?> GetSaleInvoiceByIdAsync(string salesId, string branchId, string companyId);

        Task<SalesInfo?> UpdateSaleAsync(string salesId, SalesInfo sale);
        Task<bool> DeleteSaleAsync(string salesId);

        Task<IEnumerable<SalesInfoDto>> GetSalesByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId);

    }
}
