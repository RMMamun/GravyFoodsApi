using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStocksController : ControllerBase
    {
        private readonly IProductStockRepository _repo;

        public ProductStocksController(IProductStockRepository repository)
        {
            _repo = repository;
        }

        [HttpGet("{totalProducts}/{branchId}/{companyId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<LowStockProductsDto>>>> GetLowStockProducts(int totalProducts, string branchId, string companyId)
        {
            var stocks = await _repo.GetLowStockProductsAsync(totalProducts, branchId,companyId);
            return Ok(stocks);
        }


        [HttpGet("GetStockOfAllProducts")]
        public async Task<ActionResult<ApiResponse<IEnumerable<LowStockProductsDto>>>> GetStockOfAllProducts()
        {
            var stocks = await _repo.GetAllProductStockAsync();
            return Ok(stocks);
        }



        //[HttpGet("{id}")]
        //public async Task<ActionResult<ProductStock>> Get(int id)
        //{
        //    var stock = await _repo.GetByIdAsync(id);
        //    if (stock == null) return NotFound();
        //    return Ok(stock);
        //}

        //[HttpPost]
        //public async Task<ActionResult<ProductStock>> Create(ProductStock stock)
        //{
        //    var created = await _repo.AddAsync(stock);
        //    return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, ProductStock stock)
        //{
        //    if (id != stock.Id) return BadRequest();
        //    await _repo.UpdateAsync(stock);
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
