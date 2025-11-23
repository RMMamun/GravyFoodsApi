using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
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

        [HttpGet("{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<ProductUnits>>> GetAll(string branchId, string companyId)
        {
            var units = await _repository.GetAllUnitsAsync(branchId, companyId);
            return Ok(units);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ProductUnits>>> Save(ProductUnitsDto unit)
        {
            var units = await _repository.CreateUnitAsync(unit);
            return Ok(units);
        }

    }
}
