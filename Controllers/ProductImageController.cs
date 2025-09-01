using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    public class ProductImageController : Controller
    {

        private readonly IProductImageRepository _repository;

        public ProductImageController(IProductImageRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(string id)
        {
            var product = await _repository.GetProductImagesAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(IEnumerable<ProductImageDTO> product)
        {
            var created = await _repository.SaveProductImagesAsync(product);
            return Ok(product);
            //return CreatedAtAction(nameof(Get), new { id = created.ProductId }, created);
        }

    }
}
