using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
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
    public class AuthController : ControllerBase
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

            var user = await _authService.LoginAsync(request);

            if (user == null)
                return BadRequest(new { error = "Invalid username or password" });

            return Ok(new TokenDto
            {
                Token = user.UserId
            });
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest request)
        //{
        //    // 1️⃣ LoginAsync user (includes company + branch validation)
        //    var user = await _authService.LoginAsync(request);

        //    if (user == null)
        //        return Unauthorized(new { error = "Invalid credentials" });

        //    //user.BranchId;
        //    // 2️⃣ Generate JWT access token
        //    var accessToken = _authService.GenerateTokenAsync(request);

        //    // 3️⃣ Generate refresh token
        //    var refreshToken = RefreshTokenGenerator.Generate();
        //    var refreshTokenHash = RefreshTokenGenerator.Hash(refreshToken);

        //    // 4️⃣ Store refresh token (hashed)
        //    _db.RefreshTokens.Add(new RefreshToken
        //    {
        //        UserId = user.id,
        //        TokenHash = refreshTokenHash,
        //        ExpiresAt = DateTime.UtcNow.AddDays(7),
        //        CreatedAt = DateTime.UtcNow
        //    });

        //    await _db.SaveChangesAsync();

        //    // 5️⃣ Send refresh token as HttpOnly cookie
        //    Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,               // MUST be true in production
        //        SameSite = SameSiteMode.None, // Required for WASM + API
        //        Expires = DateTime.UtcNow.AddDays(7)
        //    });

        //    // 6️⃣ Return access token only
        //    return Ok(new TokenDto
        //    {
        //        Token = accessToken
        //    });
        //}



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
