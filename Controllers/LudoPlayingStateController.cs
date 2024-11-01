using MasjidApi.MasjidRepository;
using MasjidApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasjidApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]
    public class LudoPlayingStateController : Controller
    {
        private readonly ILoggingService _logService;
        private readonly ILudoPlayingStateService _sessionServeice;
        public LudoPlayingStateController(ILudoPlayingStateService _service)
        {
            _sessionServeice = _service;
        }

        // GET: MasjidInfo/Details/5
        [HttpGet("{GetLudoPlayingStateAsync}")]
        public async Task<ActionResult<IEnumerable<LudoPlayingState>>> GetLudoPlayingStateAsync(string sessionId)
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

        [HttpPost("GetAsync")]
        public async Task<ActionResult<IEnumerable<LudoPlayingState>?>> GetAsync(string sessionid)
        {
            try
            {
                //await _logService.SaveLogAsync("Received post request in GetJoinToAGameAsync", "LudoPlayingStateController", requestDto.PlayerId.ToString());

                if (sessionid == "")
                {
                    return NotFound();
                }

                var session = await _sessionServeice.GetById(sessionid);

                if (session == null)
                {
                    await _logService.SaveLogAsync("Null result", "LudoPlayingStateController", sessionid);

                    return NotFound();
                }

                return Ok(session);

            }
            catch (Exception ex)
            {
                await _logService.SaveLogAsync(ex.Message, "LudoPlayingStateController", sessionid);

                return BadRequest(ex.Message);
            }
        }


        //[HttpPost]
        [HttpPost("Create")]
        public async Task<ActionResult<IEnumerable<LudoPlayingState>>> Create(IEnumerable<LudoPlayingState> session)
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

                var masjid = await _sessionServeice.SaveAllAsync(session);
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



        // POST: LudoPlayingStateController/Edit/5
        //public ActionResult Edit(int id, IFormCollection collection)
        [HttpPut("UpdatePlayerStatus")]
        public async Task<IActionResult> UpdatePlayerStatus([FromBody] IEnumerable<LudoPlayingState> session)
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

                var masjid = await _sessionServeice.UpdateAllAsync(session);
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



        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(IEnumerable<LudoPlayingState> ludoPlayingState)
        //{
        //    try
        //    {
        //        var Session = await _sessionServeice.Delete(ludoPlayingState);
        //        if (Session == true)
        //        {
        //            return Ok(Session);
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
