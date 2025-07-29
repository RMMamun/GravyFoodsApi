using MasjidApi.MasjidRepository;
using MasjidApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasjidApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]
    public class LudoSessionController : Controller
    {
        private readonly ILoggingService _logService;
        private readonly ILudoSessionService _sessionServeice;
        public LudoSessionController(ILudoSessionService _service)
        {
            _sessionServeice = _service;
        }

        // GET: MasjidInfo/Details/5
        [HttpGet("{GetLudoSessionAsync}")]
        public async Task<ActionResult<IEnumerable<LudoSession>>> GetLudoSessionAsync(string sessionId)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    return NotFound();
                }

                var session = await _sessionServeice.GetById(sessionId);

                if (session == null)
                {
                    return NotFound();
                }

                return Ok(session);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetJoinToAGameAsync")]
        public async Task<ActionResult<IEnumerable<LudoSession>?>> GetJoinToAGameAsync(LudoSession requestDto)
        {
            try
            {
                //await _logService.SaveLogAsync("Received post request in GetJoinToAGameAsync", "LudoSessionController", requestDto.PlayerId.ToString());

                if (requestDto == null)
                {
                    return NotFound();
                }

                var session = await _sessionServeice.APlayerWantToJoinAGame(requestDto);

                if (session == null)
                {
                    //await _logService.SaveLogAsync("Null result", "LudoSessionController", requestDto.PlayerId);

                    return NotFound();
                }

                return Ok(session);

            }
            catch (Exception ex)
            {
                await _logService.SaveLogAsync(ex.Message, "LudoSessionController", requestDto.PlayerId);

                return BadRequest(ex.Message);
            }
        }


        //[HttpPost]
        [HttpPost("Create")]
        public async Task<ActionResult<IEnumerable<LudoSession>>> Create(IEnumerable<LudoSession> session)
        {
            try
            {
                if (session == null)
                {
                    return BadRequest();
                }

                if (session != null)
                {
                    //session.MasjidID = Guid.NewGuid().ToString();
                }

                var masjid = await _sessionServeice.Create(session);
                if (masjid != null)
                {
                    return Ok(masjid);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // POST: LudoSessionController/Edit/5
        //public ActionResult Edit(int id, IFormCollection collection)
        [HttpPut("UpdateSessionStatus")]
        public async Task<IActionResult> UpdateSessionStatus([FromBody] IEnumerable<LudoSession> session)
        {
            try
            {
                if (session == null)
                {
                    return BadRequest();
                }

                if (session != null)
                {
                    //session.MasjidID = Guid.NewGuid().ToString();
                }

                var masjid = await _sessionServeice.Update(session);
                if (masjid == true)
                {
                    return Ok(masjid);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(IEnumerable<LudoSession> ludoSession)
        {
            try
            {
                var Session = await _sessionServeice.Delete(ludoSession);
                if (Session == true)
                {
                    return Ok(Session);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
