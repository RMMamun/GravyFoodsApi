using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ICustomerInfoService 
    {
        Task<ApiResponse<CustomerInfoDTO>> Create(CustomerInfoDTO customerInfo);
        Task<ApiResponse<CustomerInfoDTO?>> GetCustomerInfoById(string Id,string branchId, string companyId);
        Task<ApiResponse<CustomerInfoDTO?>> GetCustomerByMobileOrEmail(string PhoneNo, string email, string branchId, string companyId);

        Task<ApiResponse<bool>> UpdateCustomerInfoAsync(CustomerInfo dto);
        Task<ApiResponse<IEnumerable<CustomerInfoDTO>?>> GetAllCustomersAsync(string branchId, string companyId);
    }
}
