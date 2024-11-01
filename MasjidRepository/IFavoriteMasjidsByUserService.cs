using MasjidApi.Models;

namespace MasjidApi.MasjidRepository

{
    public interface IFavoriteMasjidsByUserService
    {
        Task<bool> Create(FavoriteMasjidsByUser masjid);
        Task<IEnumerable<MasjidInfo>> GetAllFavoriteMasjidbyUser(string userId);

        Task<bool> Delete(FavoriteMasjidsByUser masjid);

    }
}
