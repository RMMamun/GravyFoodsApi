using MasjidApi.DTO;
using MasjidApi.Models;

namespace MasjidApi.MasjidRepository
{
    public interface IPOSSubscription
    {
        Task<POSSubscription> Create(POSSubscription subscription);
        Task<bool> UpdateSubscriptionAsync(POSSubscription subscription);

        Task<int> CheckSubscription(SubscriptionDto subscription);
        Task<bool> isExisted(SubscriptionDto subscription);

        Task<IEnumerable<POSSubscription>?> GetAllAsync();


    }
}
