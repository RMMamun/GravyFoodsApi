using MasjidApi.DTO;
using MasjidApi.Models;

namespace MasjidApi.MasjidRepository
{
    public interface ILudoSessionService
    {
        //Task<LudoSession> GetNextUserIDAsync();

        Task<IEnumerable<LudoSession>> GetById(string sessionId);
        Task<IEnumerable<LudoSession>?> APlayerWantToJoinAGame(LudoSession requestDto);

        Task<IEnumerable<LudoSession>> Create(IEnumerable<LudoSession> ludoSession);

        Task<bool> Update(IEnumerable<LudoSession> ludoSession);
        Task<bool> Delete(IEnumerable<LudoSession> ludoSession);
    }
}
