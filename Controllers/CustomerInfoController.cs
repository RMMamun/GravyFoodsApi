using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInfoController : Controller
    {
        private readonly ICustomerInfoService _repository;

        public CustomerInfoController(ICustomerInfoService repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerInfoDTO>> Create([FromBody] CustomerInfoDTO customer)
        {
            //var created = await _repo.Create(customer);
            //return CreatedAtAction(nameof(Get), new { id = created.Id }, created);

            var customerExists = await _repository.GetCustomerByMobileOrEmail(customer.PhoneNo, customer.Email, customer.BranchId, customer.CompanyId);
            if (customerExists != null)
            {
                return Ok(customerExists);
            }

            var result = await _repository.Create(customer);

            if (!result.Success)
                return Conflict(result.Message);

            //return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, result.Data);
            return Ok(result.Data);

        }

        [HttpGet("{id}/{branchId}/{companyId}")]
        public async Task<IActionResult> GetProductById(string id, string branchId, string companyId)
        {
            var product = await _repository.GetCustomerInfoById(id,branchId,companyId);

            if (product == null)
                return NotFound(product);

            return Ok(product);
        }


        [HttpGet("{branchId}/{companyId}")]
        public async Task<IActionResult> GetProductById(string branchId, string companyId)
        {
            var product = await _repository.GetAllCustomersAsync(branchId, companyId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomerInfo([FromBody] CustomerInfoDTO dto)
        {
            var result = await _repository.UpdateCustomerInfoAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }



     }
}
