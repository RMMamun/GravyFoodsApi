using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class FavoriteMasjidsByUserService : IFavoriteMasjidsByUserService
    {
        private readonly MasjidDBContext _dbContext;
        private readonly ILogger<FavoriteMasjidsByUserService> _logger;
        public FavoriteMasjidsByUserService(MasjidDBContext dbContext, ILogger<FavoriteMasjidsByUserService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> Create(FavoriteMasjidsByUser fevmasjid)
        {

            try
            {
                var result = await _dbContext.FavoriteMasjidsByUser
                .FirstOrDefaultAsync(f => f.MasjidID == fevmasjid.MasjidID && f.UserId == fevmasjid.UserId);
                if (result != null)
                {
                    return true;
                }
                int seq = 1;

                var sequence = _dbContext.FavoriteMasjidsByUser.Any() ? _dbContext.FavoriteMasjidsByUser.Max(x => x.Sequence).ToString() : "0";

                if (sequence != null)
                {
                    seq = Convert.ToInt32(sequence) + 1;
                }
                fevmasjid.Sequence = seq;

                var masjidCreated = await _dbContext.FavoriteMasjidsByUser.AddAsync(fevmasjid);
                await _dbContext.SaveChangesAsync();
                if (masjidCreated != null)
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

        public async Task<IEnumerable<MasjidInfo>> GetAllFavoriteMasjidbyUser(string userId)
        {
            try
            {

                var query = from masjidInfo in _dbContext.MasjidInfo
                            join fevMasjid in _dbContext.FavoriteMasjidsByUser on masjidInfo.MasjidID equals fevMasjid.MasjidID
                            where fevMasjid.UserId == userId
                            select masjidInfo;

                var result = await query.ToListAsync();

                return (IEnumerable<MasjidInfo>)result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public async Task<IEnumerable<MasjidInfo>> GetAllFavoriteMasjidDtoByUser(string userId)
        //{
        //    //*** incomplete: wanted to add events, but, to add events I need date parameters from clients
        //    try
        //    {

        //        var query = from masjidInfo in _dbContext.MasjidInfo
        //                    join fevMasjid in _dbContext.FavoriteMasjidsByUser on masjidInfo.MasjidID equals fevMasjid.MasjidID
        //                    where fevMasjid.UserId == userId
        //                    select masjidInfo;

        //        var result = await query.ToListAsync();

        //        MasjidInfoDto Dto = new MasjidInfoDto();
        //        foreach (var item in result)
        //        {
        //            Dto.MasjidInfos.Add(item);

        //            List<MasjidsEvent> events = new List<MasjidsEvent>();


        //            //Dto.MasjidsEvents
        //        }

        //        return (IEnumerable<MasjidInfo>)result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}



        //public async Task<IEnumerable<MasjidInfoDto>> GetAllFavoriteMasjidbyUser(string userId)
        //{
        //    try
        //    {

        //        var query = from masjidInfo in _dbContext.MasjidInfo
        //                    join fevMasjid in _dbContext.FavoriteMasjidsByUser on masjidInfo.MasjidID equals fevMasjid.MasjidID
        //                    where fevMasjid.UserId == userId
        //                    select masjidInfo;

        //        var result = await query.ToListAsync();

        //        List<MasjidInfoDto> masjidInfoDto = new List<MasjidInfoDto>;

        //        foreach(var item in result)
        //        {
        //            masjidInfoDto.
        //        }

        //        return masjidInfoDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public async Task<bool> Delete(FavoriteMasjidsByUser masjid)
        {
            try
            {
                if (masjid == null)
                {
                    return false;
                }

                _logger.LogInformation("Before using DbContext");

                var result = await _dbContext.FavoriteMasjidsByUser
                    .FirstOrDefaultAsync(f => f.MasjidID == masjid.MasjidID && f.UserId == masjid.UserId);

                _logger.LogInformation("Before delete");

                if (result != null)
                {
                    _dbContext.FavoriteMasjidsByUser.Remove(result);
                    await _dbContext.SaveChangesAsync();

                    _logger.LogInformation("Successfully deleted");
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return false;
            }
        }


        //public async Task<bool> Delete(FavoriteMasjidsByUser masjid)
        //{
        //    try
        //    {
        //        //Int64 Id = 0;
        //        if (masjid == null)
        //        {
        //            //Id = Convert.ToInt64(strID);
        //            return false;
        //        }

        //        //var result = await _dbContext.FavoriteMasjidsByUser.Remove(x => x.UserId == masjid.UserId && x.MasjidID == masjid.MasjidID);
        //        var result = _dbContext.FavoriteMasjidsByUser
        //                        .FirstOrDefaultAsync(f => f.MasjidID == masjid.MasjidID && f.UserId == masjid.UserId);

        //        if (result != null)
        //        {
        //            //var isDelete = DeleteFav(result);
        //            var masjidDelete = _dbContext.FavoriteMasjidsByUser.Remove(await result);
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
        //        return false;
        //    }

        //}


        public async Task<bool> DeleteFav(FavoriteMasjidsByUser masjid)
        {
            try
            {
                //Int64 Id = 0;
                if (masjid == null)
                {
                    //Id = Convert.ToInt64(strID);
                    return false;
                }


                //    var masjidDelete = _dbContext.FavoriteMasjidsByUser.Remove(masjid);
                _dbContext.FavoriteMasjidsByUser.Remove(masjid);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
