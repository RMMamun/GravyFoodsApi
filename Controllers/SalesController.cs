using Azure;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace GravyFoodsApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly ITenantContextRepository _tenant;
        public SalesController(ISalesService salesService, ITenantContextRepository tenant)
        {
            _salesService = salesService;
            _tenant = tenant;
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

            companyId = _tenant.CompanyId;
            branchId = _tenant.BranchId;

            var sale = await _salesService.GetSaleByIdAsync(id, branchId, companyId);
            if (sale.Success == false) return NotFound(sale);
            return Ok(sale);
        }

        
        [HttpGet("{fromDate:Datetime}/{toDate:Datetime}/{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<SalesInfoDto>>> GetSalesByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId)
        {
            companyId = _tenant.CompanyId;
            branchId = _tenant.BranchId;

            var sale = await _salesService.GetSalesByDateRangeAsync(fromDate,toDate,branchId,companyId);
            if (sale == null) return NotFound();
            return Ok(sale);
        }


        [HttpGet("{searchStr}/{fromDate:Datetime}/{toDate:Datetime}/{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<SalesInfoDto>>> GetSalesByDateRangeAsync(string searchStr, DateTime fromDate, DateTime toDate, string branchId, string companyId)
        {
            companyId = _tenant.CompanyId;
            branchId = _tenant.BranchId;

            //var com = User.FindFirst("companyId")!.Value;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (searchStr == "-")
            {
                searchStr = "";
            }

            var sale = await _salesService.SearchSalesAsync(searchStr,fromDate, toDate, branchId, companyId);
            if (sale == null) return NotFound();
            return Ok(sale);
        }



        [HttpGet("{statusType:int}/{fromDate:Datetime}/{toDate:Datetime}/{branchId}/{companyId}")]
        public async Task<ActionResult> GetCustomSalesStatusByDateRangeAsync(int statusType, DateTime fromDate, DateTime toDate, string branchId, string companyId)
        {
            companyId = _tenant.CompanyId;
            branchId = _tenant.BranchId;

            if (statusType == 1) //Best Sold Products
            {
                var sale = await _salesService.GetBestSoldProductsByDateRangeAsync(fromDate, toDate, branchId, companyId);
                if (sale == null) return NotFound();
                return Ok(sale);

            }
            else if (statusType == 2) //Other Status
            {

            }
            else if (statusType == 3) //Other Status
            {

            }
            else if (statusType == 4) //Other Status
            {

            }


            return NotFound();
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<SalesInfoDto>>> CreateSale(SalesInfoDto sale)
        {
            sale.CompanyId = _tenant.CompanyId;
            sale.BranchId = _tenant.BranchId;


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
            sale.CompanyId = _tenant.CompanyId;
            sale.BranchId = _tenant.BranchId;

            var updated = await _salesService.UpdateSaleAsync(id, sale);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(string id)
        {
            string companyId = _tenant.CompanyId;
            string branchId = _tenant.BranchId;

            var deleted = await _salesService.DeleteSaleAsync(id,branchId,companyId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }

}
