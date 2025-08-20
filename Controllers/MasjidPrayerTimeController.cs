using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]

    public class MasjidPrayerTimeController : Controller
    {
        private readonly IMasjidPrayerTimeService _timeService;
        public MasjidPrayerTimeController(IMasjidPrayerTimeService timeService)
        {
            _timeService = timeService;
        }

        // POST: api/MasjidPrayerTime
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<bool>> CreateMasjidTime(MasjidPrayerTime fevMasjid)
        {
            try
            {
                var user = await _timeService.Create(fevMasjid);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet("GetMasjidsPrayerTime/{masjidId}")]
        public async Task<ActionResult<MasjidPrayerTime>> GetMasjidsPrayerTime(string masjidId)
        {
            try
            {
                if (string.IsNullOrEmpty(masjidId))
                {
                    return NotFound();
                }

                var masjid = await _timeService.GetMasjidPrayerTime(masjidId);

                if (masjid == null)
                {
                    return NotFound();
                }

                return Ok(masjid);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
