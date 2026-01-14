using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IAuthService
    {
        Task<string?> Authenticate(LoginRequest request);
        Task<string?> ValidateRefreshToken(string refreshToken);

        Task<string?> GenerateTokenAsync(LoginRequest user);
    }

}
