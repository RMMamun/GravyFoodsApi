using GravyFoodsApi.DTOs;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRepository _warehouseRepo;

        public WarehouseController(IWarehouseRepository warehouseRepo)
        {
            _warehouseRepo = warehouseRepo;
        }

        [HttpGet("{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<WarehouseDto>>> GetPurchase(string branchId, string companyId)
        {
            var Purchase = await _warehouseRepo.GetAllWarehousesAsync(branchId, companyId);
            return Ok(Purchase);
        }
    }
}
