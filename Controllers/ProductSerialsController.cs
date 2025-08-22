using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSerial>>> GetAll()
        {
            var serials = await _repository.GetAllAsync();
            return Ok(serials);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSerial>> Get(int id)
        {
            var serial = await _repository.GetByIdAsync(id);
            if (serial == null) return NotFound();
            return Ok(serial);
        }

        [HttpPost]
        public async Task<ActionResult<ProductSerial>> Create(ProductSerial serial)
        {
            var created = await _repository.AddAsync(serial);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductSerial serial)
        {
            if (id != serial.Id) return BadRequest();
            await _repository.UpdateAsync(serial);
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
