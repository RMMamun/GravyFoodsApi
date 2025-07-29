using MasjidApi.DTO;
using MasjidApi.MasjidRepository;
using Microsoft.AspNetCore.Mvc;

namespace MasjidApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]

    public class AssignUsersMasjidController : Controller
    {
        private readonly IUserInfoService _userService;
        public AssignUsersMasjidController(IUserInfoService userService)
        {
            _userService = userService;
        }


        [HttpPut("UpdateMasjidToUser")]
        public async Task<IActionResult> UpdateMasjidToUser(UserAsMasjidAdminDto userInfo)
        {
            try
            {
                var user = await _userService.Update_AssignMasjidToUser(userInfo);

                if (user == true)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
