using MasjidApi.Models;

namespace MasjidApi.MasjidRepository
{
    public interface IMasjidPrayerTimeService
    {
        Task<bool> Create(MasjidPrayerTime prayerTime);
        Task<MasjidPrayerTime> GetMasjidPrayerTime (string masjidId);
        Task<bool> Delete(string masjidId);
    }
}
