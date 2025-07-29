using MasjidApi.Common;
using MasjidApi.Data;
using MasjidApi.DTO;
using MasjidApi.MasjidRepository;
using MasjidApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MasjidApi.MasjidServices
{
    public class POSSubscriptionService : IPOSSubscription
    {

        CommonMethods commonMethods = new CommonMethods();
        private readonly MasjidDBContext _dbContext;
        public POSSubscriptionService(MasjidDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> isExisted(SubscriptionDto subsDto)
        {
            try
            {

                var result = await _dbContext.POSSubscription.Where(x => x.DeviceKey == subsDto.DeviceKey).FirstOrDefaultAsync();

                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> CheckSubscription(SubscriptionDto subsDto)
        {
            try
            {

                var result = await _dbContext.POSSubscription.Where(x => x.DeviceKey == subsDto.DeviceKey).FirstOrDefaultAsync();

                if (result != null)
                {
                    //int remainDays = result.SubscriptionEndDate - subsDto.SubscriptionEndDate;
                    // Step 2: Calculate the difference
                    TimeSpan difference = result.SubscriptionEndDate - subsDto.SubscriptionEndDate;

                    // Step 3: Extract the number of days from the TimeSpan
                    int daysDifference = difference.Days;

                    return daysDifference;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<POSSubscription>?> GetAllAsync()
        {
            try
            {

                var result = await _dbContext.POSSubscription.OrderBy(x => x.SubscriptionStartDate).ToListAsync();

                if (result != null)
                {
                    
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<POSSubscription> Create(POSSubscription subscription)
        {
            try
            {

                var result = await _dbContext.POSSubscription.AddAsync(subscription);
                await _dbContext.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateSubscriptionAsync(POSSubscription subs)
        {
            try
            {

                
                var result = await _dbContext.POSSubscription.Where(x => x.DeviceKey == subs.DeviceKey).FirstOrDefaultAsync();
                if (result != null)
                {

                    result.SubscriptionEndDate = subs.SubscriptionEndDate;

                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }

                
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
