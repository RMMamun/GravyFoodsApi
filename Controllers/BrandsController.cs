using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace MasjidWorldwide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _repository;

        public BrandsController(IBrandRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAll()
        {
            var brands = await _repository.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> Get(int id)
        {
            var brand = await _repository.GetBrandById(id);
            if (brand == null) return NotFound();
            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<BrandDto>> Create(BrandDto brand)
        {
            var created = await _repository.CreateAsync(brand);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, BrandDto brand)
        //{
        //    if (id != brand.Id) return BadRequest();

        //    await _repository.UpdateAsync(brand);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _repository.DeleteAsync(id);
        //    return NoContent();
        //}
    }
}
