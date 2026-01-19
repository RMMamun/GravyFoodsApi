using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyInfoController : Controller
    {
        private readonly ICompanyInfoService _repository;

        public CompanyInfoController(ICompanyInfoService repository)
        {
            _repository = repository;
        }


        //[HttpPost]
        //public async Task<ActionResult<CustomerInfo>> Create([FromBody] CustomerInfoDTO customer)
        //{
        //    //var created = await _repo.Create(customer);
        //    //return CreatedAtAction(nameof(Get), new { id = created.Id }, created);

        //    var customerExists = await _repo.GetCustomerByMobileOrEmail(customer.PhoneNo, customer.Email, customer.BranchId, customer.CompanyId);
        //    if (customerExists != null)
        //    {
        //        return Ok(customerExists);
        //    }

        //    var result = await _repo.Create(customer);

        //    if (!result.Success)
        //        return Conflict(result.Message);

        //    return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, result.Data);
        //    //return Ok(result.Data);

        //}

        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetProductById(string companyId)
        {
            var product = await _repository.GetCompanyInfoAsync(companyId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }


        [HttpGet("{regCode:guid}")]
        public async Task<IActionResult> GetCompanyRegistrationVerification(Guid regCode)
        {
            var result = await _repository.GetCompanyRegistrationVerificationAsync(regCode);
            if (result.Success == false)
                return NotFound(result.Message);
            return Ok(result);
        }

    }
}
