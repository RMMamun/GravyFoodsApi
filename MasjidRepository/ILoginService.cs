using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ILoginService
    {
        Task<LoginRequest?> GetUser(string username, string password, string branchId, string companyCode);
    }
}
