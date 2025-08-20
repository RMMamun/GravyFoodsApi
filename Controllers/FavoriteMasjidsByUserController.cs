using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]

    public class FavoriteMasjidsByUserController : Controller
    {

        private readonly IFavoriteMasjidsByUserService _fevService;
        private readonly IMasjidInfoService _masjidService;
        public FavoriteMasjidsByUserController(IFavoriteMasjidsByUserService fevService, IMasjidInfoService masjidService)
        {
            _fevService = fevService;
            _masjidService = masjidService;
        }

        // POST: api/FavoriteMasjidsByUser
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<bool>> CreateFevMasjid(FavoriteMasjidsByUser fevMasjid)
        {
            try
            {
                var user = await _fevService.Create(fevMasjid);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // GET: MasjidInfo/Details/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<MasjidInfo>>> GetAllMasjidsbyUser(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound();
                }

                var masjid = await _fevService.GetAllFavoriteMasjidbyUser(userId);

                //var masjid = await _context.MasjidInfo.FindAsync(masjidid);
                //var masjid = await _context.MasjidInfo.Where(x => x.MasjidID == masjidid).FirstOrDefaultAsync();

                //var masjid = await _context.MasjidInfo.FirstOrDefaultAsync(x => x.MasjidID == masjidid);

                //.Select(x => MasjidDTO(x))
                //.ToListAsync();

                if (masjid == null)
                {
                    return NotFound();
                }

                foreach (var masj in masjid)
                {
                    if (masj.ImagePath != "")
                    {
                        byte[] masjidImage = await _masjidService.GetImageDataFromDevice(masj.ImagePath);
                        masj.ImageAsByte = masjidImage;
                    }
                }

                return Ok(masjid);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/FavoriteMasjidsByUser
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("DeleteFevMasjid")]
        public async Task<ActionResult<bool>> DeleteFevMasjid(FavoriteMasjidsByUser favMasjib)
        {
            try
            {
                var user = await _fevService.Delete(favMasjib);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
