using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ILoggingService
    {
        Task<bool> Create(Logging log);
        Task<bool> SaveLogAsync(string description, string scource, string userid);
    }
}
