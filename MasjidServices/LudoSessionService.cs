using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class LudoSessionService : ILudoSessionService
    {
        //CommonMethods commonMethods = new CommonMethods();
        private readonly MasjidDBContext _dbContext;
        private readonly ILoggingService _logService;

        ILudoPlayingStateService _playingStateService;
        public LudoSessionService(MasjidDBContext dbContext, ILoggingService loggingService, ILudoPlayingStateService playingStateService)
        {
            _dbContext = dbContext;
            _logService = loggingService;
            _playingStateService = playingStateService;
        }

        public async Task<IEnumerable<LudoSession>> GetById(string sessionId)
        {
            try
            {
                var session = await _dbContext.LudoSession.Where(x => x.SessionId == sessionId && x.isActive == "True").ToListAsync();
                if (session == null)
                {
                    return null;
                }
                else
                {
                    //if (session.PlayerImageAsByte != "")
                    //{
                    //    byte[] masjidImage = await GetImageDataFromDevice(session.ImagePath);
                    //    session.ImageAsByte = masjidImage;
                    //}

                    return session;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<LudoSession>? GetFreeSlotGame(LudoSession requestDto)
        {

            try
            {
                LudoSession session = null;
                DateTime fromdatetime = DateTime.Now.AddMinutes(-10);
                DateTime nowdatetime = DateTime.Now;

                if (requestDto.MappingId == "")
                {
                    //get own session lists
                    var sessionidList = await _dbContext.LudoSession.Where(x => x.PlayerId == requestDto.PlayerId && x.isActive == "True" && x.Sequence == 1).ToListAsync();

                    //skip own sessions and get other players session
                    if (sessionidList.Count() == 0)
                    {
                        session = await _dbContext.LudoSession.Where(x => x.PlayerId == "Unmapped" && x.isActive == "True" && x.GameDateTime >= fromdatetime && x.GameDateTime <= nowdatetime).FirstOrDefaultAsync();
                    }
                    else
                    {
                        session = await _dbContext.LudoSession.Where(x => x.PlayerId == "Unmapped" && x.isActive == "True" && x.GameDateTime >= fromdatetime && x.GameDateTime <= nowdatetime &&
                                !sessionidList.Select(s => s.SessionId).Contains(x.SessionId)).FirstOrDefaultAsync();
                    }
                }
                else
                {
                    session = await _dbContext.LudoSession.Where(x => x.MappingId == requestDto.MappingId && x.PlayerId == "Unmapped" && x.isActive == "True").FirstOrDefaultAsync();
                }


                if (session != null)
                {
                    //session.SessionId = requestDto.SessionId;
                    //session.MappingId = requestDto.MappingId;
                    //session.Sequence = requestDto.Sequence;
                    session.PlayerName = requestDto.PlayerName;
                    session.PlayerId = requestDto.PlayerId;
                    session.WorldRank = requestDto.WorldRank;
                    session.Points = requestDto.Points;
                    session.isActive = "True";
                    session.PlayerImageAsByte = requestDto.PlayerImageAsByte;

                    await _dbContext.SaveChangesAsync();

                    return session;
                }
                else
                {

                    return null;
                }
                //return session;

            }
            catch (Exception ex)
            {
                await _logService.SaveLogAsync(ex.Message, "GetFreeSlotGame", requestDto.PlayerId);
                return null;
            }
        }
        public async Task<IEnumerable<LudoSession>?> APlayerWantToJoinAGame(LudoSession requestDto)
        {
            try
            {
                //_logger.LogInformation("entered to the method APlayerWantToJoinAGame");

                //Logging newlog = new Logging
                //{
                //    Id = 1,
                //    LogDescription = "entered to the method APlayerWantToJoinAGame",
                //    SourceName = "LudoSessionService",
                //    UserId = requestDto.PlayerId,
                //    EntryDateTime = DateTime.UtcNow
                //};
                //await _logService.Create(newlog);

                //LudoSession session = null;

                bool isUpdated = false;

                var session = await GetFreeSlotGame(requestDto);

                if (session == null)
                {
                    await _logService.SaveLogAsync("No free slot found", "APlayerWantToJoinAGame", requestDto.PlayerId);
                    return null;
                }
                else
                {
                    isUpdated = true;
                    requestDto.MappingId = session.MappingId;
                    requestDto.SessionId = session.SessionId;
                    requestDto.isActive = session.isActive;
                }


                //if (session.PlayerImageAsByte != "")
                //{
                //    byte[] masjidImage = await GetImageDataFromDevice(session.ImagePath);
                //    session.ImageAsByte = masjidImage;
                //}


                //var isUpdated = await UpdatePlayerAsync(requestDto);

                if (isUpdated == true)
                {
                    var allplayers = await _dbContext.LudoSession.Where(x => x.SessionId == requestDto.SessionId && x.isActive == "True").ToListAsync();
                    if (allplayers != null)
                    {
                        return allplayers;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    await _logService.SaveLogAsync("Could not save", "APlayerWantToJoinAGame", requestDto.PlayerId);

                    return null;
                }


            }
            catch (Exception ex)
            {
                await _logService.SaveLogAsync(ex.Message, "APlayerWantToJoinAGame", requestDto.PlayerId);

                return null;
            }
        }


        public async Task<IEnumerable<LudoSession>?> Create(IEnumerable<LudoSession> ludoSession)
        {
            try
            {

                var sessionId = "";
                var mappingId = "";

                //Delete all previous Active sessions of the player which was not completed successfully. 
                var p1 = ludoSession.Where(x => x.Sequence == 1).FirstOrDefault();
                if (p1 != null)
                {
                    await DeleteAllActiveSessionByUser(p1.PlayerId, p1.SessionId);
                }
                //

                foreach (var session in ludoSession.OrderBy(x => x.Sequence))
                {

                    if (sessionId == "")
                    {
                        sessionId = await GetNextSessionIDAsync();

                        //if (session.SessionId == "0" || session.SessionId == "")
                        //{
                        //    sessionId = await GetNextSessionIDAsync();
                        //}
                        //else
                        //{
                        //    sessionId = session.SessionId;
                        //}
                    }

                    if (session.MappingId == "")
                    {
                        var newSession = new LudoSession
                        {
                            SessionId = sessionId,
                            PlayerId = session.PlayerId,
                            PlayerName = session.PlayerName,
                            WorldRank = session.WorldRank,
                            Points = session.Points,
                            MappingId = session.Sequence.ToString() + sessionId,
                            isActive = "True",
                            PlayerImageAsByte = session.PlayerImageAsByte,
                            Sequence = session.Sequence,
                            GameDateTime = session.GameDateTime

                        };

                        await _dbContext.LudoSession.AddAsync(newSession);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                //await SaveImage(ludoSession.ImageByteData);


                var result = await _dbContext.LudoSession.Where(x => x.SessionId == sessionId).OrderBy(o => o.Sequence).ToListAsync();
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

        public async Task<bool> Update(IEnumerable<LudoSession> ludoSession)
        {
            try
            {

                foreach (var session in ludoSession)
                {
                    //var updateSession = await _dbContext.LudoSession.FindAsync(session.SessionId);
                    var updateSession = await _dbContext.LudoSession.Where(x => x.SessionId == session.SessionId && x.PlayerId == session.PlayerId).FirstOrDefaultAsync();

                    if (updateSession == null)
                    {
                        return false;
                    }

                    //updateSession.SessionId = session.SessionId;
                    updateSession.PlayerId = session.PlayerId;
                    updateSession.MappingId = session.MappingId;
                    updateSession.isActive = session.isActive;
                    updateSession.PlayerImageAsByte = session.PlayerImageAsByte;

                    await _dbContext.SaveChangesAsync();


                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public async Task<bool> UpdatePlayerAsync(LudoSession session)
        //{
        //    try
        //    {
        //        bool isNew = false;
        //        var ludo = await _dbContext.LudoSession.Where(x => x.SessionId == session.SessionId && x.MappingId == session.MappingId).FirstOrDefaultAsync();
        //        if (ludo != null)
        //        {
        //            ludo.SessionId = session.SessionId;
        //            ludo.MappingId = session.MappingId;
        //            ludo.PlayerName = session.PlayerName;
        //            ludo.PlayerId = session.PlayerId;
        //            ludo.Sequence = session.Sequence;
        //            ludo.WorldRank = session.WorldRank;
        //            ludo.Points = session.Points;
        //            ludo.isActive = "True";
        //            ludo.PlayerImageAsByte = session.PlayerImageAsByte;

        //            await _dbContext.SaveChangesAsync();

        //            return true;
        //        }
        //        else
        //        {

        //            return false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        await _logService.SaveLogAsync(ex.Message, "UpdatePlayerAsync", session.PlayerId);

        //        return false;
        //    }
        //}

        //private async Task<bool> UpdatePlayerAsync1(LudoSession session)
        //{
        //    try
        //    {
        //        //var updateSession = await _dbContext.LudoSession.FindAsync(session.SessionId);
        //        var getPlayer = await _dbContext.LudoSession.Where(x => x.SessionId == session.SessionId && x.MappingId == session.MappingId && x.PlayerId == "Unmapped" && x.isActive == "True").FirstOrDefaultAsync();

        //        bool isDeleted = await DeleteById(getPlayer.Id);
        //        if (isDeleted == false)
        //        {
        //            return false;
        //        }

        //        if (getPlayer != null)
        //        {
        //            var updatePlayer = await _dbContext.LudoSession.Where(x => x.Id == getPlayer.Id).FirstOrDefaultAsync();
        //            if (updatePlayer == null)
        //            {
        //                return false;
        //            }

        //            updatePlayer.Id = updatePlayer.Id;
        //            updatePlayer.SessionId = session.SessionId;
        //            updatePlayer.MappingId = session.MappingId;
        //            updatePlayer.PlayerId = session.PlayerId;
        //            updatePlayer.PlayerName = session.PlayerName;
        //            updatePlayer.WorldRank = session.WorldRank;
        //            updatePlayer.Points = session.Points;
        //            updatePlayer.isActive = session.isActive;
        //            updatePlayer.PlayerImageAsByte = session.PlayerImageAsByte;

        //            //_dbContext.LudoSession.Remove(getPlayer);
        //            //await _dbContext.SaveChangesAsync();

        //            //_dbContext.LudoSession.Update(updatePlayer);
        //            //await _dbContext.SaveChangesAsync();

        //            _dbContext.LudoSession.Attach(updatePlayer);
        //            _dbContext.Entry(updatePlayer).State = EntityState.Modified;
        //            await _dbContext.SaveChangesAsync();


        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Logging newlog = new Logging
        //        {
        //            Id = 1,
        //            LogDescription = ex.Message,
        //            SourceName = this.ToString() + "\\" + "UpdatePlayerAsync",
        //            UserId = session.PlayerId,
        //            EntryDateTime = DateTime.UtcNow
        //        };
        //        await _logService.Create(newlog);

        //        return false;

        //    }
        //}

        public async Task<bool> DeleteById(long ID)
        {
            try
            {
                if (ID == 0)
                {
                    return false;
                }

                //_logger.LogInformation("Before using DbContext");


                var result = await _dbContext.LudoSession
                    .FirstOrDefaultAsync(f => f.Id == ID);

                //_logger.LogInformation("Before delete");

                if (result != null)
                {
                    _dbContext.LudoSession.Remove(result);
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

        public async Task<bool> Delete(IEnumerable<LudoSession> ludoSession)
        {
            try
            {
                if (ludoSession == null)
                {
                    return false;
                }

                //_logger.LogInformation("Before using DbContext");

                foreach (var session in ludoSession)
                {
                    var result = await _dbContext.LudoSession
                        .FirstOrDefaultAsync(f => f.SessionId == session.SessionId && f.Sequence == session.Sequence);

                    //_logger.LogInformation("Before delete");

                    if (result != null)
                    {
                        _dbContext.LudoSession.Remove(result);
                        await _dbContext.SaveChangesAsync();

                        //_logger.LogInformation("Successfully deleted");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return false;
            }
        }


        public async Task<bool> DeleteAllActiveSessionByUser(string playerid, string sessionid)
        {
            try
            {
                var ludoSession = await _dbContext.LudoSession
                        .Where(f => f.PlayerId == playerid && f.SessionId != sessionid && f.Sequence == 1 && f.isActive == "True").ToListAsync();
                if (ludoSession.Count == 0)
                {
                    return false;
                }

                //_logger.LogInformation("Before using DbContext");


                foreach (var session in ludoSession)
                {
                    var result = await _dbContext.LudoSession
                        .Where(f => f.SessionId == session.SessionId && f.SessionId != sessionid && f.isActive == "True").ToListAsync();

                    //_logger.LogInformation("Before delete");

                    if (result != null)
                    {
                        _dbContext.LudoSession.RemoveRange(result);
                        await _dbContext.SaveChangesAsync();

                        //_logger.LogInformation("Successfully deleted");
                    }


                    //also delete the plyers state of this session
                    await _playingStateService.DeleteById(session.SessionId);
                }


                //delete all invalid sessions
                var sessionIds = await _dbContext.LudoSession.Where(w => w.isActive == "True")
                                .GroupBy(x => x.SessionId)
                                .Where(g => g.Count() == 1)
                                .Select(g => g.Key)
                                .ToListAsync();


                foreach (var session in sessionIds)
                {
                    var result = await _dbContext.LudoSession
                        .Where(f => f.SessionId == session && f.isActive == "True").ToListAsync();

                    //_logger.LogInformation("Before delete");

                    if (result != null)
                    {
                        _dbContext.LudoSession.RemoveRange(result);
                        await _dbContext.SaveChangesAsync();

                        //_logger.LogInformation("Successfully deleted");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return false;
            }
        }


        public async Task<string> GetNextSessionIDAsync()
        {
            try
            {
                //var maxId = _dbContext.LudoSession.Max(u => u.Id);
                var maxId = _dbContext.LudoSession.Any() ? _dbContext.LudoSession.Max(u => u.Id) : 1;

                maxId = maxId == 0 ? 1 : maxId + 1;
                string formattedString = maxId.ToString();
                formattedString = formattedString.PadLeft(10, '0');
                return formattedString;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return "";
            }
        }



    }
}
