using MasjidApi.Common;
using MasjidApi.Data;
using MasjidApi.MasjidRepository;
using Microsoft.EntityFrameworkCore;
using MasjidApi.Models;
using MasjidApi.DTO;
using System.Collections.Generic;

namespace MasjidApi.MasjidServices
{
    public class MasjidEventService : IMasjidsEventService
    {

        CommonMethods commonMethods = new CommonMethods();
        private readonly MasjidDBContext _dbContext;
        public MasjidEventService(MasjidDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MasjidsEvent> Create(MasjidsEvent? masjidsEvent)
        {
            try
            {
                //if (user.UserName == "@N*e^w?U/s>e$r#")
                //{
                //    var maxId = NextUserID();

                //    user.UserId = "User" + maxId.ToString();
                //    user.UserName = user.UserId;

                //    const string chars1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                //    const string chars2 = "0123456789!@#$%^&*()ABCDEFGHIJKL0123456789!@#$%^&*()MNOPQRSTUVWXY0123456789!@#$%^&*()Z0123456789!@#$%^&*()abcdefghijklmnop0123456789!@#$%^&*()qrstuvwxyz0123456789!@#$%^&*()";
                //    Random random = new Random();

                //    var Password = new string(Enumerable.Repeat(chars2, 12).Select(s => s[random.Next(s.Length)]).ToArray());

                //    user.Password = Password;    //commonMethods.GenerateRandomString(10);
                //}

                var result = await _dbContext.MasjidsEvent.AddAsync(masjidsEvent);
                await _dbContext.SaveChangesAsync();


                return masjidsEvent;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<MasjidsEvent> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MasjidsEvent>> GetAllEventsByMasjidID(string masjidid)
        {
            //get all events by event id
            try
            {
                //return await _dbContext.MasjidsEvent.FirstAsync
                var result = await _dbContext.MasjidsEvent.Where(x => x.MasjidID == masjidid).ToListAsync();
                return result;

            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<MasjidsEvent>> GetAllEventsAsOnDate(MasjidEventParamDto Dto)
        {
            try
            {
                
                var result = await _dbContext.MasjidsEvent.Where(x => x.MasjidID == Dto.MasjidID && Dto.StartDate >= x.EventStartDate && Dto.StartDate <= x.EventEndDate).ToListAsync();
                return result;

            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<EventListDto>> GetAllEventsByUserAndDate(string userId,DateTime eventEndDate)
        {
            try
            {
                //var userId = "User60039";
                //var eventEndDate = new DateTime(2024, 05, 23);

                var query = from u in _dbContext.UserInfo
                            join fm in _dbContext.FavoriteMasjidsByUser on u.UserId equals fm.UserId into ufm
                            from fm in ufm.DefaultIfEmpty()
                            join mi in _dbContext.MasjidInfo on fm.MasjidID equals mi.MasjidID
                            join me in _dbContext.MasjidsEvent on fm.MasjidID equals me.MasjidID
                            where u.UserId == userId && me.EventEndDate >= eventEndDate
                            select new
                            {
                                me.EventId,
                                mi.MasjidID,
                                me.EventTitle,
                                me.EventDescription,
                                me.EventStartDate,
                                me.EventEndDate,
                                me.EntryDateTime,
                                u.UserId,
                                mi.MasjidName,


                            };

                var result = query.ToList();

                List<EventListDto> eventLists = new List<EventListDto>();

                foreach ( var item in result )
                {
                    var eventListDto = new EventListDto
                    {
                        UserId = item.UserId,
                        MasjidID = item.MasjidID,
                        MasjidName = item.MasjidName,
                        EventId = item.EventId,
                        EventTitle = item.EventTitle,
                        EventDescription = item.EventDescription,
                        EventStartDate = item.EventStartDate,
                        EventEndDate = item.EventEndDate,
                        EntryDateTime = item.EntryDateTime
                        
                    };

                    eventLists.Add(eventListDto);
                }

                return eventLists;

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public Task<MasjidsEvent> Update(MasjidsEvent masjidsEvent)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<EventTypes>> GetAllEventTypes()
        {
            //get all events by event id
            try
            {
                //return await _dbContext.MasjidsEvent.FirstAsync
                var result = await _dbContext.EventTypes.ToListAsync();
                return result;

            }
            catch
            {
                return null;
            }
        }
    }
}
