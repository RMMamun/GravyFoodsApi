using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantsController : ControllerBase
    {
        private readonly IProductVariantRepository _repository;

        public ProductVariantsController(IProductVariantRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVariant>>> GetAll()
        {
            var variants = await _repository.GetAllAsync();
            return Ok(variants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVariant>> Get(int id)
        {
            var variant = await _repository.GetByIdAsync(id);
            if (variant == null) return NotFound();
            return Ok(variant);
        }

        [HttpPost]
        public async Task<ActionResult<ProductVariant>> Create(ProductVariant variant)
        {
            var created = await _repository.AddAsync(variant);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductVariant variant)
        {
            if (id != variant.Id) return BadRequest();
            await _repository.UpdateAsync(variant);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
