using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace MasjidWorldwide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _repository;

        public BrandsController(IBrandRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BrandDto>>>> GetAll()
        {
            var brands = await _repository.GetBrandsAsync();
            if (brands.Success == false) return NotFound(brands);
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BrandDto>>> Get(int id)
        {
            var brand = await _repository.GetBrandById(id);
            if (brand.Success == false) return NotFound(brand);
            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<BrandDto>>> Create(BrandDto brand)
        {
            var created = await _repository.CreateAsync(brand);
            if (!created.Success)
            {
                return BadRequest(created);
            }

            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, BrandDto brand)
        {
            //if (id != brand.Id) return BadRequest();

            var result = await _repository.UpdateAsync(brand);
            if (!result.Success)
            {  
                return BadRequest(result); 
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
