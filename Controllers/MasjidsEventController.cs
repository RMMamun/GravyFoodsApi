using GravyFoodsApi.DTO;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]

    public class MasjidsEventController : Controller
    {
        private readonly IMasjidsEventService _eventService;

        public MasjidsEventController(IMasjidsEventService eventService)
        {
            _eventService = eventService;
        }


        [HttpGet("GetAllEventByMasjidId")]
        public async Task<ActionResult<IEnumerable<MasjidsEvent>>> GetAllEventByMasjidId(string masjidID)
        {

            try
            {
                if (masjidID == null)
                {
                    return NotFound();
                }

                var allEvents = await _eventService.GetAllEventsByMasjidID(masjidID);

                if (allEvents == null)
                {
                    return NotFound();
                }

                return Ok(allEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllEventsAsOnDate")]
        public async Task<ActionResult<IEnumerable<MasjidsEvent>>> GetAllEventsAsOnDate(string masjidID, DateTime AsOnDateTime)
        {

            try
            {

                if (string.IsNullOrEmpty(masjidID) == true || (AsOnDateTime == null || AsOnDateTime == default(DateTime)))
                {
                    return NotFound();
                }

                MasjidEventParamDto Dto = new MasjidEventParamDto();
                Dto.MasjidID = masjidID;
                Dto.StartDate = AsOnDateTime;
                Dto.EndDate = AsOnDateTime;


                var allEvents = await _eventService.GetAllEventsAsOnDate(Dto);

                if (allEvents == null)
                {
                    return NotFound();
                }

                return Ok(allEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllEventsByUserAndDate")]
        public async Task<ActionResult<IEnumerable<EventListDto>>> GetAllEventsByUserAndDate(string userId, DateTime AsOnDateTime)
        {

            try
            {

                if (string.IsNullOrEmpty(userId) == true || (AsOnDateTime == null || AsOnDateTime == default(DateTime)))
                {
                    return NotFound();
                }

                var allEvents = await _eventService.GetAllEventsByUserAndDate(userId, AsOnDateTime);

                if (allEvents == null)
                {
                    return NotFound();
                }

                return Ok(allEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult<MasjidsEvent>> Create(MasjidsEvent addEvent)
        {
            try
            {
                if (addEvent == null)
                {
                    return BadRequest();
                }

                //addEvent.EventId = Guid.NewGuid().ToString();

                var masjidEvent = await _eventService.Create(addEvent);
                if (masjidEvent != null)
                {
                    return CreatedAtAction(nameof(Create), new { EventId = masjidEvent.EventId }, masjidEvent);
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }



        [HttpGet("GetAllEventTypes")]
        public async Task<ActionResult<IEnumerable<MasjidsEvent>>> GetAllEventTypes()
        {

            try
            {
                var allEvents = await _eventService.GetAllEventTypes();

                if (allEvents == null)
                {
                    return NotFound();
                }

                return Ok(allEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
