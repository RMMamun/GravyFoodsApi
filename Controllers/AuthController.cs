using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;
using System.Threading.Tasks;

namespace GravyFoodsApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : PosBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            //
            //request.CompanyCode = TenantId.ToString(); // Set CompanyCode from TenantId  *** ERROR on registry/context reading


            var token = await _authService.Authenticate(request);

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { error = "Invalid username or password" });

            return Ok(new LoginResponse
            {
                Token = token
            });
        }

        public class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
        }


        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            return Ok(new { message = $"Hello {User.Identity?.Name}, here is your secret data!" });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var user = _authService.ValidateRefreshToken(refreshToken);
            if (user == null)
                return Unauthorized();

            LoginRequest request = new LoginRequest();
            //set user to request
            var newAccessToken = await _authService.GenerateTokenAsync(request);

            return Ok(new { accessToken = newAccessToken });
        }


    }


}
