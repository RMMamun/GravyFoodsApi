using MasjidApi.DTO;
using MasjidApi.MasjidRepository;
using MasjidApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MasjidApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]
    public class LoggingController : Controller
    {
        private readonly ILoggingService _logService;

        public LoggingController(ILoggingService logService)
        {
            _logService = logService;
        }

        // POST: api/UserInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create")]
        public async Task<ActionResult<bool>> Create(Logging log)
        {
            try
            {
                if (log == null)
                {
                    return NotFound();
                }

                var user = await _logService.Create(log);
                return true;
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
