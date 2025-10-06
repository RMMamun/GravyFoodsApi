using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ISupplierRepository 
    {

        Task<SupplierInfo> Create(SupplierDTO supplierInfo);
        Task<SupplierInfo?> GetSupplierInfoById(string Id, string branchId, string companyId);
        Task<SupplierInfo?> GetSupplierByMobileOrEmail(string PhoneNo, string email, string branchId, string companyId);

        Task<bool> UpdateSupplierInfoAsync(SupplierDTO supplierInfo);

        Task<bool> DeleteAsync(string id, string branchId, string companyId);
        Task<IEnumerable<SupplierInfo>?> GetAllSuppliersAsync(string branchId, string companyId);
    }
}
