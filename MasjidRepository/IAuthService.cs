using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IAuthService
    {
        string? Authenticate(LoginRequest request);
    }

}
