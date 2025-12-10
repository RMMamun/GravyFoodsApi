using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MasjidWorldwide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }




        [HttpGet("{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(string branchId, string companyId)
        {
            var products = await _repository.GetProductsWithDetailsAsync(branchId, companyId);
            return Ok(products);
        }

        [HttpGet("{id}/{branchId}/{companyId}")]
        public async Task<ActionResult<ProductDto>> Get(string id, string branchId, string companyId)
        {
            var product = await _repository.GetProductByIdAsync(id, branchId, companyId);
            if (product == null) return NotFound();
            return Ok(product);
        }

        //[HttpGet("{barcode}/{branchId}/{companyId}")]
        //public async Task<ActionResult<ProductDto>> Get(string barcode, string branchId, string companyId)
        //{
        //    var product = await _repo.GetProductByBarcodeAsync(barcode, branchId, companyId);
        //    if (product == null) return NotFound();
        //    return Ok(product);
        //}


        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductDto product)
        {
            var created = await _repository.AddProductAsync(product);
            //return CreatedAtAction(nameof(Get), new { id = created.ProductId }, created);

            if (created.Success == false)
            {
                //return NotFound();
                return BadRequest(created);

            }

            //return Ok(created);
            return Ok(created);


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ProductDto product)
        {
            if (id != product.ProductId) return BadRequest();

            bool updated = await _repository.UpdateProductByIdAsync(product);
            if (updated == true)
                return Ok(updated);
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}/{branchId}/{companyId}")]
        public async Task<IActionResult> Delete(string id, string branchId, string companyId)
        {
            await _repository.DeleteProductAsync(id, branchId, companyId);
            return NoContent();
        }
    }
}
