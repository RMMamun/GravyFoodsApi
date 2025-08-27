using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductUnitController : Controller
    {
        private readonly IProductUnitRepository _repository;

        public ProductUnitController(IProductUnitRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductUnits>>> GetAll()
        {
            var units = await _repository.GetAllUnitsAsync();
            return Ok(units);
        }
    }
}
