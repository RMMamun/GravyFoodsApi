using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SupplierController : Controller
    {

        private readonly ISupplierRepository _repository;

        public SupplierController(ISupplierRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<SupplierInfo>> Create([FromBody] SupplierDTO supplier)
        {
            //var created = await _repository.Create(supplier);
            //return CreatedAtAction(nameof(Get), new { id = created.Id }, created);

            var result = await _repository.Create(supplier);

            //if (!result.Success)
            //    return Conflict(result.Message);

            //return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, result.Data);
            return Ok(result);

        }

        [HttpPut]
        public async Task<ActionResult<SupplierInfo>> UpdateAsync([FromBody] SupplierDTO supplier)
        {
            //var created = await _repository.Create(supplier);
            //return CreatedAtAction(nameof(Get), new { id = created.Id }, created);

            var result = await _repository.UpdateSupplierInfoAsync(supplier);

            //if (!result.Success)
            //    return Conflict(result.Message);

            //return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, result.Data);
            return Ok(result);

        }


        [HttpGet("{id}/{branchId}/{companyId}")]
        public async Task<IActionResult> GetSupplierById(string id, string branchId, string companyId)
        {
            var product = await _repository.GetSupplierInfoById(id,branchId,companyId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }



        [HttpGet("{branchId}/{companyId}")]
        public async Task<IActionResult> GetAllSupplierAsync(string branchId, string companyId)
        {
            var product = await _repository.GetAllSuppliersAsync(branchId, companyId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpDelete("{id}/{branchId}/{companyId}")]
        public async Task<IActionResult> DeleteAsync(string id, string branchId, string companyId)
        {
            bool product = await _repository.DeleteAsync(id,branchId,companyId);

            if (product == false)
                return NotFound();

            return Ok(product);
        }

        



    }
}
