using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ILudoPlayingStateService
    {
        Task<IEnumerable<LudoPlayingState>?> GetBySessionId(string sessionId);

        Task<LudoPlayingState>? SaveAsync(LudoPlayingState ludoPlayingState);

        Task<IEnumerable<LudoPlayingState>?> SaveAllAsync(IEnumerable<LudoPlayingState> allPlayersState);

        Task<bool> UpdateAsync(LudoPlayingState ludoPlayingState);

        Task<bool> UpdateAllAsync(IEnumerable<LudoPlayingState> allPlayersState);
        Task<bool> UpdateDiceTurnAsync(IEnumerable<LudoPlayingState> allPlayersState);

        Task<bool> DeleteById(string sessionId);

    }
}
