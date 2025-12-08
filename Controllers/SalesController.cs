using Azure;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

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

        [HttpGet("{id}/{branchId}/{companyId}")]
        public async Task<ActionResult<ApiResponse<SalesInfoDto>>> GetSale(string id, string branchId, string companyId)
        {
            var sale = await _salesService.GetSaleByIdAsync(id, branchId, companyId);
            if (sale.Success == false) return NotFound(sale);
            return Ok(sale);
        }

        
        [HttpGet("{fromDate:Datetime}/{toDate:Datetime}/{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<SalesInfoDto>>> GetSalesByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId)
        {
            var sale = await _salesService.GetSalesByDateRangeAsync(fromDate,toDate,branchId,companyId);
            if (sale == null) return NotFound();
            return Ok(sale);
        }
        

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SalesInfoDto>>> CreateSale(SalesInfoDto sale)
        {

            var apiResponse = await _salesService.CreateSalesAsync(sale);

            if (apiResponse.Success == false)
            {
                return BadRequest(apiResponse);
            }

            //return CreatedAtAction(nameof(GetSale), new { id = created.SalesId }, created);
            return Ok(apiResponse);

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
