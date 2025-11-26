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

        //[HttpPut]
        //public async Task<ActionResult<bool>> Update(ProductUnitsDto unit)
        //{
        //    var isExisted = _repository.GetUnitsById(unit.UnitId, unit.BranchId, unit.CompanyId);
        //    if (isExisted == null)
        //    {
        //        return NotFound("Unit not found");
        //    }

        //    var units = await _repository.UpdateUnitAsync(unit);
        //    return Ok(units);
        //}

        [HttpPut]
        public async Task<IActionResult> Update(ProductUnitsDto unit)
        {
            var existed = await _repository.GetUnitsById(unit.UnitId, unit.BranchId, unit.CompanyId);
            if (existed == null)
                return NotFound("Unit not found");

            var success = await _repository.UpdateUnitAsync(unit);
            return Ok(success);
        }

        [HttpDelete("{unitId}/{branchId}/{companyId}")]
        public async Task<ActionResult<bool>> Delete(string unitId, string branchId, string companyId)
        {
            var isExisted = await _repository.GetUnitsById(unitId, branchId, companyId);
            if (isExisted == null)
            {
                return NotFound("Unit not found");
            }
            var units = await _repository.DeleteUnitAsync(unitId, branchId, companyId);
            return Ok(units);
        }





    }
}
