using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ILoginService
    {
        LoginRequest? GetUser(string username, string password);
    }
}
