using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable<PurchaseInfoDto>> GetAllPurchaseAsync();
        Task<PurchaseInfo?> GetPurchaseByIdAsync(string PurchaseId, string branchId, string companyId);
        Task<PurchaseInfoDto> CreatePurchaseAsync(PurchaseInfoDto Purchase);
        Task<PurchaseInfo?> UpdatePurchaseAsync(string PurchaseId, PurchaseInfo Purchase);
        Task<bool> DeletePurchaseAsync(string PurchaseId, string branchId, string companyId);

        Task<IEnumerable<PurchaseInfoDto>> GetPurchaseByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId);

    }


}
