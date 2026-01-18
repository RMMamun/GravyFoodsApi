using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ILoginService
    {
        Task<UserInfoDTO?> GetUser(string username, string password, string branchId, string companyCode);
    }
}
