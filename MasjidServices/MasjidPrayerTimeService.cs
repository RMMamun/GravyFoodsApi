using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class MasjidPrayerTimeService : IMasjidPrayerTimeService
    {
        private readonly MasjidDBContext _dbContext;
        public MasjidPrayerTimeService(MasjidDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(MasjidPrayerTime prayerTime)
        {
            try
            {
                var isExists = await _dbContext.MasjidPrayerTime.Where(x => x.MasjidID == prayerTime.MasjidID).FirstOrDefaultAsync();

                if (isExists == null)
                {
                    await _dbContext.MasjidPrayerTime.AddAsync(prayerTime);
                }
                else
                {
                    //_dbContext.Update(prayerTime);
                    _dbContext.Entry(isExists).State = EntityState.Detached;
                    _dbContext.Entry(prayerTime).State = EntityState.Modified;

                }

                await _dbContext.SaveChangesAsync();


                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<bool> Delete(string masjidId)
        {
            throw new NotImplementedException();
        }

        public async Task<MasjidPrayerTime> GetMasjidPrayerTime(string masjidId)
        {
            var result = await _dbContext.MasjidPrayerTime.Where(x => x.MasjidID == masjidId).FirstOrDefaultAsync();
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
