using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IMasjidPrayerTimeService
    {
        Task<bool> Create(MasjidPrayerTime prayerTime);
        Task<MasjidPrayerTime> GetMasjidPrayerTime(string masjidId);
        Task<bool> Delete(string masjidId);
    }
}
