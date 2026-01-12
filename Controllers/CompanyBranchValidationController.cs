using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyBranchValidationController : Controller
    {
        private readonly IBranchInfoRepository _repository;
        private readonly IContextCookieService _cookieService;

        public CompanyBranchValidationController(IBranchInfoRepository repository, IContextCookieService cookieService)
        {
            _repository = repository;
            _cookieService = cookieService;
        }

        [HttpGet("{regCode}")]
        public async Task<IActionResult> RegValidation(string regCode)
        {
            try
            {
                var product = await _repository.GetLinkCodeVerificationAsync(regCode);

                //implementation not completed yet 
                _cookieService.SetBranchContext(Response, (product.Data.CompanyCode), (product.Data.BranchCode));

                if (product == null)
                    return NotFound();

                return Ok(product);
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
