using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStocksController : ControllerBase
    {
        private readonly IProductStockRepository _repository;

        public ProductStocksController(IProductStockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStock>>> GetAll()
        {
            var stocks = await _repository.GetAllAsync();
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductStock>> Get(int id)
        {
            var stock = await _repository.GetByIdAsync(id);
            if (stock == null) return NotFound();
            return Ok(stock);
        }

        [HttpPost]
        public async Task<ActionResult<ProductStock>> Create(ProductStock stock)
        {
            var created = await _repository.AddAsync(stock);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductStock stock)
        {
            if (id != stock.Id) return BadRequest();
            await _repository.UpdateAsync(stock);
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
