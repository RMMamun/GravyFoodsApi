using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using System.ComponentModel.Design;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSerialsController : ControllerBase
    {
        private readonly IProductSerialRepository _repository;

        public ProductSerialsController(IProductSerialRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{productId},{branchId},{companyId}")]
        public async Task<ActionResult<IEnumerable<ProductSerial>>> GetAll(string productId, string branchId, string companyId)
        {
            var serials = await _repository.GetProductSerialsByProductIdAsync(productId, branchId, companyId);
            return Ok(serials);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<ProductSerial>> Get(int id)
        //{
        //    var serial = await _repo.GetByIdAsync(id);
        //    if (serial == null) return NotFound();
        //    return Ok(serial);
        //}

        [HttpPost]
        public async Task<ActionResult<bool>> Create(IEnumerable<ProductSerialDto> serial)
        {
            var created = await _repository.AddProductSerialAsync(serial);
            if (created.Success == false)
            {
                return BadRequest(created);
            }

            return Ok(created);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, ProductSerial serial)
        //{
        //    if (id != serial.Id) return BadRequest();
        //    await _repo.UpdateAsync(serial);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _repo.DeleteAsync(id);
        //    return NoContent();
        //}
    }
}
