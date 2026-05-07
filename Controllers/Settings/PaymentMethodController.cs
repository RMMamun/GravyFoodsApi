using GravyFoodsApi.MasjidRepository.Settings;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : Controller
    {
        
        private readonly IPaymentMethodRepository _repo;

        public PaymentMethodController(IPaymentMethodRepository repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PaymentMethodsDto>>>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            if (result.Success == false) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<PaymentMethodsDto>>> GetByIdAsync(Guid id)
        {
            var brand = await _repo.GetByIdAsync(id);

            if (!brand.Success)
                return NotFound(brand);

            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PaymentMethodsDto>>> Create(PaymentMethodsDto brand)
        {
            var created = await _repo.CreateAsync(brand);
            if (!created.Success)
            {
                return BadRequest(created);
            }

            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, PaymentMethodsDto brand)
        {
            //if (id != brand.Id) return BadRequest();

            var result = await _repo.UpdateAsync(brand);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
        {
            var result = await _repo.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
