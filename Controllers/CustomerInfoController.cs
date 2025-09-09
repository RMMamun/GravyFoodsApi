using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
using GravyFoodsApi.Models;
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
        public async Task<ActionResult<CustomerInfo>> Create([FromBody] CustomerInfoDTO customer)
        {
            //var created = await _repository.Create(customer);
            //return CreatedAtAction(nameof(Get), new { id = created.Id }, created);


            var result = await _repository.Create(customer);

            if (!result.Success)
                return Conflict(result.Message);

            return CreatedAtAction(nameof(GetProductById), new { id = result.Data.Id }, result.Data);
            //return Ok(result.Data);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _repository.GetCustomerInfoById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }





    }
}
