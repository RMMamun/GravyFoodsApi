using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using MasjidApi.Models;
using MasjidApi.Data;
using MasjidApi.MasjidRepository;
using MasjidApi.DTO;

namespace MasjidApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]
    public class UserInfoController : ControllerBase
    {
        //private readonly MasjidDBContext _context;
        private readonly IUserInfoService _userService;

        public UserInfoController(IUserInfoService userService)
        {
            _userService = userService;
        }
        //public UserInfoController(MasjidDBContext context)
        //{
        //    _context = context;
        //}


        // GET: api/UserInfo
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfo()
        //{
        //    try
        //    {
        //        //return await _context.UserInfo.Select(x => UserToDTO(x)).ToListAsync();
        //        return await _context.UserInfo.ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        // GET: api/UserInfo/5
        [HttpGet("{userid}")]
        //public async Task<ActionResult<UserInfoDTO>> GetUserInfo(long userid)
        public async Task<ActionResult<UserInfoDTO>> GetUserInfo(string userid)
        {
            var userInfo = await _userService.GetUserById(userid);

            if (userInfo == null)
            {
                return NotFound();
            }

            return UserToDTO(userInfo);
        }

        public class LoginUser
        {
            public string userid { get; set; }
            public string password { get; set; }
            public string DeviceId { get; set; }

        }
        

        // PUT: api/UserInfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userid}")]
        public async Task<IActionResult> UpdateUserInfo(string userid, UserInfoDTO UserInfoDTO)
        {
            if (userid != UserInfoDTO.UserId)
            {
                return BadRequest();
            }

            //var UserInfo = await _context.UserInfo.FindAsync(userid);
            //if (UserInfo == null)
            //{
            //    return NotFound();
            //}

            //UserInfo.UserName = UserInfoDTO.UserName;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException) when (!UserInfoExists(userid))
            //{
            //    return NotFound();
            //}

            return NoContent();
        }

        // POST: api/UserInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserInfo>> CreateUserInfo(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                return BadRequest();
            }

            try
            {
                var user = await _userService.Create(userInfo);

                return user;
                //return CreatedAtAction(
                //    nameof(GetUserInfo), new { UserId = userInfo.UserId },
                //    UserToDTO(user));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        



        ////For Login
        //// POST: api/UserInfo
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost("login")]
        //public async Task<ActionResult<UserLogin>> CreateUserInfo(UserLogin userLogin)
        //{
        //    try
        //    {
        //        //var UserInfo = await _context.UserInfo.FindAsync(userLogin.UserId);
        //        //if (UserInfo == null)
        //        //{
        //        //    return NotFound();
        //        //}

        //        //if (UserInfo.UserId == userLogin.UserId && UserInfo.Password == userLogin.Password)
        //        //{
        //        //    return Ok();
        //        //}
        //        //else
        //        //{
        //            return NotFound();
        //        //}                
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

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
            //var UserInfo = await _context.UserInfo.FindAsync(userid);

            //if (UserInfo == null)
            //{
            //    return NotFound();
            //}

            //_context.UserInfo.Remove(UserInfo);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserInfoExists(string userid)
        {
            //return _context.UserInfo.Any(e => e.UserId == userid);
            return false;
        }

        private static UserInfoDTO UserToDTO(UserInfo userInfo) =>
            new UserInfoDTO
            {
                UserId = userInfo.UserId,
                UserName = userInfo.UserName,
                Password = userInfo.Password,
                DeviceId = userInfo.DeviceId,
                UserRole = userInfo.UserRole,
                MasjidID = userInfo.MasjidID,
                Latitude = userInfo.Latitude,
                Longitude   = userInfo.Longitude
                
            };
    }
}