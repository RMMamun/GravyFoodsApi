using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributesController : ControllerBase
    {
        private readonly IProductAttributeRepository _repository;

        public ProductAttributesController(IProductAttributeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductAttribute>>> GetAll()
        {
            var attributes = await _repository.GetAllAsync();
            return Ok(attributes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductAttribute>> Get(int id)
        {
            var attribute = await _repository.GetByIdAsync(id);
            if (attribute == null) return NotFound();
            return Ok(attribute);
        }

        [HttpPost]
        public async Task<ActionResult<ProductAttribute>> Create(ProductAttribute attribute)
        {
            var created = await _repository.AddAsync(attribute);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductAttribute attribute)
        {
            if (id != attribute.Id) return BadRequest();
            await _repository.UpdateAsync(attribute);
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
