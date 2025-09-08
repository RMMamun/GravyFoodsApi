using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ICustomerInfoService : IRepository<CustomerInfo>
    {
        Task<ServiceResultWrapper<CustomerInfo>> Create(CustomerInfoDTO customerInfo);
        Task<CustomerInfo?> GetCustomerInfoById(int Id);
        Task<bool> UpdateCustomerInfoAsync(CustomerInfo customerInfo);
        Task<IEnumerable<CustomerInfo>?> GetAllCustomersAsync();
    }
}
