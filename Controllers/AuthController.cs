using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

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
        public IActionResult Login([FromBody] LoginRequest request)
        {
            //
            //request.CompanyCode = TenantId.ToString(); // Set CompanyCode from TenantId  *** ERROR on registry/context reading
            

            var token = _authService.Authenticate(request);
            if (token == null) return BadRequest(new { error = "Invalid username or password" });

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            return Ok(new { message = $"Hello {User.Identity?.Name}, here is your secret data!" });
        }
    }

}
