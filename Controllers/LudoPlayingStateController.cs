using GravyFoodsApi.DTO;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
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

                var session = await _sessionServeice.GetBySessionId(sessionId);

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

                var session = await _sessionServeice.GetBySessionId(sessionid);

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
                    //diceTurn.MasjidID = Guid.NewGuid().ToString();
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
                    //diceTurn.MasjidID = Guid.NewGuid().ToString();
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


        [HttpPut("UpdateDiceTurnAsync")]
        public async Task<IActionResult> UpdateDiceTurnAsync([FromBody] DiceTurnDto diceTurn)
        {
            try
            {
                if (diceTurn == null)
                {
                    return BadRequest();
                }

                //Note: if diceTurn = True, it will update the seleted user as True and all others will be false, but if the value is False then it will only update the selected player.

                List<LudoPlayingState> allPlayersState = new List<LudoPlayingState>();

                var currentState = await _sessionServeice.GetBySessionId(diceTurn.SessionId);
                if (currentState != null)
                {
                    foreach (var player in currentState)
                    {
                        LudoPlayingState newState = new LudoPlayingState()
                        {
                            SessionId = player.SessionId,
                            isActive = player.isActive,
                            isPlayerActive = player.isPlayerActive,
                            PlayerId = player.PlayerId,
                            MappingId = player.MappingId,
                            MyTurn = player.MyTurn,
                            DiceValue = player.DiceValue,
                            SelectedValue = player.SelectedValue,
                            SelectedBall = player.SelectedBall,
                            wasRead = player.wasRead
                        };

                        if (newState.MappingId == diceTurn.MappingId)
                        {
                            newState.MyTurn = diceTurn.MyTurn;
                        }
                        else
                        {
                            if (diceTurn.MyTurn == "True")
                            {
                                newState.MyTurn = "False";
                            }

                        }

                        allPlayersState.Add(newState);
                    }
                }
                else
                {
                    return BadRequest();
                }

                var masjid = await _sessionServeice.UpdateDiceTurnAsync(allPlayersState);
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
