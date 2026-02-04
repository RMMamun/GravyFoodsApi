using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasjidWorldwide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryRepository _repo;

        public ProductCategoriesController(IProductCategoryRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductCategoryDto>>>> GetAll()
        {
            var result = await _repo.GetAllCategoriesAsync();
            if (result.Success == false) return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductCategoryDto>>> Get(int id)
        {
            var result = await _repo.GetCategoryById(id);
            if (result.Success == false) return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductCategoryDto>>> Create(ProductCategoryDto category)
        {
            var result = await _repo.CreateCategoryAsync(category);
            if (result.Success == false) return NotFound(result);

            return Ok(result);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, ProductCategoryDto category)
        //{
        //    if (id != category.Id) return BadRequest();

        //    var result = await _repo.up(category);
        //    if (result == null) return NotFound(result);

        //    return Ok(result);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _repo.DeleteAsync(id);
        //    return NoContent();
        //}



        [HttpGet("tree")]
        public async Task<ActionResult<ApiResponse<CategoryTreeDto>>> GetTree()
        {
            var result = await _repo.GetCategoryTreeAsync();
            if (result.Success == false) return NotFound(result);

            return Ok(result);
        }


    }
}
