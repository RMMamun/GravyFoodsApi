using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
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
        private readonly ITenantContextRepository _tenant;

        public AuthController(IAuthService authService, ITenantContextRepository tenant)
        {
            _authService = authService;
            _tenant = tenant;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            //
            //request.CompanyCode = TenantId.ToString(); // Set CompanyCode from TenantId  *** ERROR on registry/context reading
            var companyid = _tenant.CompanyId;
            var branchid = _tenant.BranchId;
            
            request.CompanyCode = companyid ?? string.Empty;
            request.BranchCode = branchid ?? string.Empty;


            var token = await _authService.Authenticate(request);

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { error = "Invalid username or password" });

            return Ok(new TokenDto
            {
                Token = token
            });
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
