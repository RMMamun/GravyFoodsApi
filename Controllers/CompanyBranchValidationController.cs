using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyBranchValidationController : Controller
    {
        private readonly IBranchInfoRepository _repository;
        private readonly IContextCookieService _cookieService;
        private readonly IAuthService _authService;

        public CompanyBranchValidationController(IBranchInfoRepository repository, IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        [HttpGet("{regCode}")]
        public async Task<IActionResult> RegValidation(string regCode)
        {
            try
            {
                var result = await _repository.GetLinkCodeVerificationAsync(regCode);

                //implementation not completed yet 
                //_cookieService.SetBranchContext(Response, (result.Data.CompanyCode), (result.Data.BranchCode));


                if (result == null)
                    return NotFound();

                LoginRequest loginRequest = new LoginRequest();
                loginRequest.BranchCode = result.Data.BranchCode;
                loginRequest.CompanyCode = result.Data.CompanyCode;
                loginRequest.Username = "";

                var token = await _authService.GenerateTokenAsync(loginRequest);


                //return Ok(token);
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new { error = "Invalid username or password" });

                return Ok(new TokenDto
                {
                    Token = token
                });


            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here for brevity)
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult CheckRegistration()
        {
            if (HttpContext.Items["TenantId"] == null)
                return Unauthorized();

            return Ok();
        }


    }
}
