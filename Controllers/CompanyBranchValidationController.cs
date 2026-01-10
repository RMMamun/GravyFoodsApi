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

        [HttpGet("{linkCode}")]
        public async Task<IActionResult> GetBranchById(string linkCode)
        {
            var product = await _repository.GetLinkCodeVerificationAsync(linkCode);

            _cookieService.SetBranchContext(Response, Guid.Parse(product.Data.CompanyCode), Guid.Parse(product.Data.BranchCode));

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
