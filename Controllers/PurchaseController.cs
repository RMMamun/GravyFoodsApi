using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _PurchaseService;

        public PurchaseController(IPurchaseRepository PurchaseService)
        {
            _PurchaseService = PurchaseService;
        }

        [HttpGet("{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<PurchaseInfoDto>>> GetPurchase(string branchId, string companyId)
        {
            var Purchase = await _PurchaseService.GetAllPurchaseAsync(branchId, companyId);
            return Ok(Purchase);
        }

        [HttpGet("{id}/{branchId}/{companyId}")]
        public async Task<ActionResult<PurchaseInfo>> GetPurchase(string id, string branchId, string companyId)
        {
            var Purchase = await _PurchaseService.GetPurchaseByIdAsync(id, branchId,companyId);
            if (Purchase == null) return NotFound();
            return Ok(Purchase);
        }


        [HttpGet("{fromDate:Datetime}/{toDate:Datetime}/{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<PurchaseInfoDto>>> GetPurchaseByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId)
        {
            var Purchase = await _PurchaseService.GetPurchaseByDateRangeAsync(fromDate, toDate, branchId, companyId);
            if (Purchase == null) return NotFound();
            return Ok(Purchase);
        }


        [HttpPost]
        public async Task<ActionResult<PurchaseInfoDto>> CreatePurchase(PurchaseInfoDto Purchase)
        {
            var created = await _PurchaseService.CreatePurchaseAsync(Purchase);
            return CreatedAtAction(nameof(GetPurchase), new { id = created.PurchaseId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PurchaseInfo>> UpdatePurchase(string id, PurchaseInfo Purchase)
        {
            var updated = await _PurchaseService.UpdatePurchaseAsync(id, Purchase);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}/{branchId}/{companyId}")]
        public async Task<IActionResult> DeletePurchase(string id, string branchId, string companyId)
        {
            var deleted = await _PurchaseService.DeletePurchaseAsync(id, branchId, companyId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }

}
