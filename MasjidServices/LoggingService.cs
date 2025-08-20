using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidServices
{
    public class LoggingService : ILoggingService
    {
        private readonly MasjidDBContext _dbContext;
        public LoggingService(MasjidDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(Logging log)
        {
            try
            {
                //Logging newlog = new Logging { LogDescription = sDescription, SourceName = sSource, UserId = sUser, EntryDateTime = DateTime.UtcNow };

                var result = await _dbContext.Logging.AddAsync(log);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SaveLogAsync(string description, string scource, string userid)
        {
            try
            {
                Logging newlog = new Logging { LogDescription = description, SourceName = scource, UserId = userid, EntryDateTime = DateTime.UtcNow };

                var result = await Create(newlog);

                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
