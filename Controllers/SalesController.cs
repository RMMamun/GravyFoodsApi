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
        private readonly IProductStockRepository _StockRepo;
        private readonly IUnitConversionRepository _unitConvRepo;
        public SalesController(ISalesService salesService, IProductStockRepository stockRepo, IUnitConversionRepository unitConvRepo)
        {
            _salesService = salesService;
            _StockRepo = stockRepo;
            _unitConvRepo = unitConvRepo;
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

        
        [HttpGet("{fromDate:Datetime}/{toDate:Datetime}/{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<SalesInfoDto>>> GetSalesByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId)
        {
            var sale = await _salesService.GetSalesByDateRangeAsync(fromDate,toDate,branchId,companyId);
            if (sale == null) return NotFound();
            return Ok(sale);
        }
        

        [HttpPost]
        public async Task<ActionResult<SalesInfoDto>> CreateSale(SalesInfoDto sale)
        {
            var created = await _salesService.CreateSaleAsync(sale);

            //Update Stock after creating Sale
            var stockUpdate = await StockUpdate(sale);
            if (!stockUpdate.Success)
            {
                //return BadRequest(stockUpdate.Message);
            }

            return CreatedAtAction(nameof(GetSale), new { id = created.SalesId }, created);
        }


        private async Task<APIResponseDto> StockUpdate(SalesInfoDto sale)
        { 
            APIResponseDto response = new APIResponseDto();

            try   
            {
                ProductStockDto stock = new ProductStockDto();
                foreach (var item in sale.SalesDetails)
                {
                    
                    response = await _StockRepo.UpdateProductStockAsync(item.ProductId, item.Quantity,item.UnitType,item.WHId,item.BranchId,item.CompanyId);
                }

                response.Success = true;
                response.Message = "Stock updated successfully.";
                return response;
                // Your stock update logic here

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while updating stock." + Environment.NewLine + ex.Message;
                return response;
            }   
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
