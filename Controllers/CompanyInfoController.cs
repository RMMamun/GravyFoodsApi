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
        //    //var created = await _repository.Create(customer);
        //    //return CreatedAtAction(nameof(Get), new { id = created.Id }, created);

        //    var customerExists = await _repository.GetCustomerByMobileOrEmail(customer.PhoneNo, customer.Email, customer.BranchId, customer.CompanyId);
        //    if (customerExists != null)
        //    {
        //        return Ok(customerExists);
        //    }

        //    var result = await _repository.Create(customer);

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



    }
}
