using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginRequest request);
        Task<string?> ValidateRefreshToken(string refreshToken);

        Task<string?> GenerateTokenAsync(LoginRequest user);
    }

}
