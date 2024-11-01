using MasjidApi.DTO;
using MasjidApi.Models;

namespace MasjidApi.MasjidRepository
{
    public interface IPOSSubscription
    {
        Task<POSSubscription> Create(POSSubscription subscription);

        Task<int> CheckSubscription(SubscriptionDto subscription);
        Task<bool> isExisted(SubscriptionDto subscription);
        

    }
}
