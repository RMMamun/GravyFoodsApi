using MasjidApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasjidApi.MasjidRepository
{
    public interface ILudoPlayingStateService 
    {
        Task<IEnumerable<LudoPlayingState>?> GetById(string sessionId);

        Task<LudoPlayingState>? SaveAsync(LudoPlayingState ludoPlayingState);

        Task<IEnumerable<LudoPlayingState>?> SaveAllAsync(IEnumerable<LudoPlayingState> allPlayersState);

        Task<bool> UpdateAsync(LudoPlayingState ludoPlayingState);

        Task<bool> UpdateAllAsync(IEnumerable<LudoPlayingState> allPlayersState);

        Task<bool> DeleteById(string sessionId);

    }
}
