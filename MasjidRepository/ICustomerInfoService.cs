using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ICustomerInfoService 
    {
        Task<ApiResponse<CustomerInfoDTO>> Create(CustomerInfoDTO customerInfo);
        Task<ApiResponse<CustomerInfoDTO?>> GetCustomerInfoById(string Id);
        Task<ApiResponse<CustomerInfoDTO?>> GetCustomerByMobileOrEmail(string PhoneNo, string email);

        Task<ApiResponse<bool>> UpdateCustomerInfoAsync(CustomerInfoDTO dto);
        Task<ApiResponse<IEnumerable<CustomerInfoDTO>?>> GetAllCustomersAsync(string branchId);
    }
}
