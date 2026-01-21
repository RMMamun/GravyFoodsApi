using GravyFoodsApi.DTO;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ISubscriptionInfo
    {
        Task<SubscriptionInfo> Create(SubscriptionInfo subscription);
        Task<bool> UpdateSubscriptionAsync(SubscriptionInfo subscription);

        Task<int> CheckSubscription(SubscriptionDto subscription);
        Task<bool> isExisted(SubscriptionDto subscription);

        Task<IEnumerable<SubscriptionInfo>?> GetAllAsync();

        Task<ApiResponse<CompanyRegistrationResponseDto?>> GetCompanyRegistrationVerificationAsync(string RegCode);




    }
}
