using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitPriceConversionController : Controller
    {
        private readonly IUnitPriceConversionRepository _repo;

        public UnitPriceConversionController(IUnitPriceConversionRepository repository)
        {
            _repo = repository;
        }


        [HttpGet("{productId}")]
        public async Task<ActionResult<ApiResponse<List<ProductUnitsWithPriceDto>>>> GetProductUnitsWithPriceAsync(string productId)
        {
            try
            {
                var units = await _repo.GetProductUnitsWithPriceAsync(productId);
                return Ok(units);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<ProductUnitsWithPriceDto>>
                {
                    Success = false,
                    Message = "Error retrieving units with price: " + ex.Message,
                    Data = new List<ProductUnitsWithPriceDto>()
                });
            }
        }
    }
}
