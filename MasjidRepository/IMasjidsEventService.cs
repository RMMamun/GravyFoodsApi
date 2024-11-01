using MasjidApi.DTO;
using MasjidApi.Models;


namespace MasjidApi.MasjidRepository
{
    public interface IMasjidsEventService
    {
        Task<MasjidsEvent> Create(MasjidsEvent masjidsEvent);
        Task<MasjidsEvent> Update(MasjidsEvent masjidsEvent);
        Task<MasjidsEvent> Delete(long id);
        Task<IEnumerable<MasjidsEvent>> GetAllEventsByMasjidID(string masjidID);
        Task<IEnumerable<MasjidsEvent>> GetAllEventsAsOnDate(MasjidEventParamDto masjidEventDto);

        Task<IEnumerable<EventTypes>> GetAllEventTypes();

        Task<IEnumerable<EventListDto>> GetAllEventsByUserAndDate(string userId, DateTime eventEndDate);
    }
}
