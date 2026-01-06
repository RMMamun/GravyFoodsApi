using GravyFoodsApi.MasjidRepository;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppOptionsController : Controller
    {
        private readonly IAppOptionsRepository _repository;

        public AppOptionsController(IAppOptionsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{branchId}/{companyId}")]
        public async Task<IActionResult> GetAllAppOptions(string branchId, string companyId)
        {
            var product = await _repository.GetAppOptionsAsync(branchId, companyId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
