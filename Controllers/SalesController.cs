using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesInfoDto>>> GetSales()
        {
            var sales = await _salesService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesInfo>> GetSale(string id)
        {
            var sale = await _salesService.GetSaleByIdAsync(id);
            if (sale == null) return NotFound();
            return Ok(sale);
        }

        [HttpPost]
        public async Task<ActionResult<SalesInfoDto>> CreateSale(SalesInfoDto sale)
        {
            var created = await _salesService.CreateSaleAsync(sale);
            return CreatedAtAction(nameof(GetSale), new { id = created.SalesId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SalesInfo>> UpdateSale(string id, SalesInfo sale)
        {
            var updated = await _salesService.UpdateSaleAsync(id, sale);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(string id)
        {
            var deleted = await _salesService.DeleteSaleAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }

}
