using MasjidApi.Data;
using MasjidApi.MasjidRepository;
using MasjidApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MasjidApi.MasjidServices
{
    public class LudoPlayingStateService : ILudoPlayingStateService
    {
        private readonly MasjidDBContext _dbContext;
        private readonly ILoggingService _logService;
        public LudoPlayingStateService(MasjidDBContext dbContext, ILoggingService loggingService)
        {
            _dbContext = dbContext;
            _logService = loggingService;
        }

        public async Task<IEnumerable<LudoPlayingState>?> GetBySessionId(string sessionId)
        {
            try
            {
                var session = await _dbContext.LudoPlayingState.Where(x => x.SessionId == sessionId && x.isActive == "True").ToListAsync();
                if (session == null)
                {
                    return null;
                }
                else
                {
                    return session;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<LudoPlayingState>?> SaveAllAsync(IEnumerable<LudoPlayingState> allPlayersState)
        {
            try
            {

                var sessionId = "";
                var mappingId = "";

                //Delete all previous Active sessions of the player which was not completed successfully. 

                //var p1 = allPlayersState.Where(x => x.isActive == "True").FirstOrDefault();
                //if (p1 != null)
                //{
                //    await DeleteAllActiveSessionByUser(p1.PlayerId, p1.SessionId);
                //}
                //

                foreach (var session in allPlayersState)
                {

                    if (sessionId == "")
                    {
                        sessionId = session.SessionId;
                        var isdelete = await DeleteById(sessionId);
                    }

                    if (session.SessionId != "")
                    {
                        var playerStatus = new LudoPlayingState
                        {
                            SessionId = session.SessionId,
                            isActive = "True",
                            PlayerId = session.PlayerId,
                            MappingId = session.MappingId,
                            isPlayerActive = session.isPlayerActive,
                            MyTurn = session.MyTurn,
                            DiceValue = session.DiceValue,
                            SelectedValue = session.SelectedValue,
                            SelectedBall = session.SelectedBall,
                            wasRead = session.wasRead

                        };

                        await _dbContext.LudoPlayingState.AddAsync(playerStatus);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                //await SaveImage(allPlayersState.ImageByteData);


                var result = await _dbContext.LudoPlayingState.Where(x => x.SessionId == sessionId).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }


                //}

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<LudoPlayingState>? SaveAsync(LudoPlayingState ludoPlayingState)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAllAsync(IEnumerable<LudoPlayingState> allPlayersState)
        {
            try
            {

                foreach (var player in allPlayersState)
                {
                    var updatePlayer = await _dbContext.LudoPlayingState.Where(x => x.SessionId == player.SessionId && x.PlayerId == player.PlayerId).FirstOrDefaultAsync();
                    if (updatePlayer == null)
                    {
                        return false;
                    }

                    //updatePlayer.SessionId = player.SessionId;
                    updatePlayer.PlayerId = player.PlayerId;
                    updatePlayer.MappingId = player.MappingId;
                    updatePlayer.isPlayerActive = player.isPlayerActive;
                    updatePlayer.isActive = player.isActive;
                    updatePlayer.DiceValue = player.DiceValue;
                    updatePlayer.SelectedValue = player.SelectedValue;
                    updatePlayer.SelectedBall = player.SelectedBall;
                    updatePlayer.wasRead = player.wasRead;

                    updatePlayer.MyTurn = updatePlayer.MyTurn;  //keep it as it is, its update by other method

                    await _dbContext.SaveChangesAsync();


                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateDiceTurnAsync(IEnumerable<LudoPlayingState> allPlayersState)
        {
            try
            {

                foreach (var player in allPlayersState)
                {
                    var updatePlayer = await _dbContext.LudoPlayingState.Where(x => x.SessionId == player.SessionId && x.PlayerId == player.PlayerId).FirstOrDefaultAsync();
                    if (updatePlayer == null)
                    {
                        return false;
                    }

                    //updatePlayer.SessionId = player.SessionId;
                    updatePlayer.PlayerId = player.PlayerId;
                    updatePlayer.MappingId = player.MappingId;
                    updatePlayer.isPlayerActive = player.isPlayerActive;
                    updatePlayer.isActive = player.isActive;
                    updatePlayer.MyTurn = player.MyTurn;
                    updatePlayer.DiceValue = player.DiceValue;
                    updatePlayer.SelectedValue = player.SelectedValue;
                    updatePlayer.SelectedBall = player.SelectedBall;
                    updatePlayer.wasRead = player.wasRead;

                    await _dbContext.SaveChangesAsync();


                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<bool> UpdateAsync(LudoPlayingState ludoPlayingState)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteById(string sessionId)
        {
            try
            {
                if (sessionId == "")
                {
                    return false;
                }

                //_logger.LogInformation("Before using DbContext");


                var result = await _dbContext.LudoPlayingState
                    .Where(f => f.SessionId == sessionId).ToListAsync();

                //_logger.LogInformation("Before delete");

                if (result.Count() > 0)
                {
                    _dbContext.LudoPlayingState.RemoveRange(result);
                    await _dbContext.SaveChangesAsync();

                    //_logger.LogInformation("Successfully deleted");
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return false;
            }
        }

    }
}
