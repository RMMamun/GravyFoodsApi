using MasjidApi.Data;
using MasjidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasjidApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly MasjidDBContext _context;

        public LoginController(MasjidDBContext context)
        {
            _context = context;
        }

        public class LoginUser
        {
            public string userid { get; set; }
            public string password { get; set; }
            public string deviceId { get; set; }
        }

        // POST: api/UserInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserInfo>> CreateUserInfo(LoginUser login)
        {
            try
            {
                UserInfo? user = await _context.UserInfo.FirstOrDefaultAsync(x => x.UserId == login.userid && x.Password == login.password && x.DeviceId == login.deviceId);
                //return CreatedAtAction( nameof(GetUserInfo), new { UserId = userInfo.UserId },
                //    UserToDTO(userInfo));

                if (user == null)
                {
                    return Ok(new List<UserInfo>());
                }
                else
                {
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //For Login
        // POST: api/UserInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("login")]
        public async Task<ActionResult<UserLogin>> UserLoginAsync(LoginUser userLogin)
        {
            try
            {
                UserInfo? user = await _context.UserInfo.FirstOrDefaultAsync(x => x.UserId == userLogin.userid && x.Password == userLogin.password && x.DeviceId == userLogin.deviceId);
                //return CreatedAtAction( nameof(GetUserInfo), new { UserId = userInfo.UserId },
                //    UserToDTO(userInfo));

                if (user == null)
                {
                    return Ok(new List<UserInfo>());
                }
                else
                {
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return null;
            }


            //try
            //{
            //    var UserInfo = await _context.UserInfo.FindAsync(userLogin.UserId);
            //    if (UserInfo == null)
            //    {
            //        return NotFound();
            //    }

            //    if (UserInfo.UserId == userLogin.UserId && UserInfo.Password == userLogin.Password)
            //    {
            //        return Ok();
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }

        public class UserLogin
        {
            public string UserId { get; set; }
            public string Password { get; set; }
        }

        // POST: api/UserInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserInfoDTO>> CreateUserInfo(string userId,string userName, string pass)
        //{
        //    try
        //    {
        //        var userInfo = new UserInfo
        //        {
        //            //Id = userInfoDTO.Id,
        //            UserId = userId,
        //            UserName = userName,
        //            Password = pass
        //        };

        //        _context.UserInfo.Add(userInfo);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction(
        //            nameof(GetUserInfo),
        //            new { UserId = userInfo.UserId },
        //            UserToDTO(userInfo));
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        // DELETE: api/UserInfo/5
        [HttpDelete("{userid}")]
        public async Task<IActionResult> DeleteUserInfo(string userid)
        {
            var UserInfo = await _context.UserInfo.FindAsync(userid);

            if (UserInfo == null)
            {
                return NotFound();
            }

            _context.UserInfo.Remove(UserInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserInfoExists(string userid)
        {
            return _context.UserInfo.Any(e => e.UserId == userid);
        }

        private static UserInfoDTO UserToDTO(UserInfo userInfo) =>
            new UserInfoDTO
            {
                UserId = userInfo.UserId,
                UserName = userInfo.UserName,
                Password = userInfo.Password,
                DeviceId = userInfo.DeviceId,
                UserRole = userInfo.UserRole,
                MasjidID = userInfo.MasjidID

            };
    }
}