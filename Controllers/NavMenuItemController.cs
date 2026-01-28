using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace GravyFoodsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class NavMenuItemController : Controller
    {
        private readonly INavMenuRepository _repo;
        private readonly ITenantContextRepository _tenant;
        public NavMenuItemController(INavMenuRepository repo, ITenantContextRepository tenant)
        {
            _repo = repo;
            _tenant = tenant;
        }


            

        [HttpGet]
        public async Task<ActionResult<ApiResponse<bool>>> GetHierarchicalMenus()
        {
            try
            {
                var menus = await _repo.GetHierarchicalMenusAsync();

                return Ok(menus);
            }
            catch (Exception ex)
            {
                ApiResponse<bool> response = new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Error fetching hierarchical menus",
                    Data = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(response);
            }
            
        }

        [HttpGet("parents")]
        public async Task<ActionResult<ApiResponse<IEnumerable<NavMenuItemDto>>>> GetParentMenus()
        {
            try
            {
                var parents = await _repo.GetParentMenusAsync();

                return Ok(parents);
            }
            catch (Exception ex)
            {
                ApiResponse<IEnumerable<NavMenuItemDto>> response = new ApiResponse<IEnumerable<NavMenuItemDto>>
                {
                    Success = false,
                    Message = "Error fetching parent menus",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(response);
            }
        }

        [HttpGet("menuitems/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<NavMenuItemDto>>>> GetMenuItems(string userId)
        {
            try
            {
                var menus = await _repo.GetMenusByUserAsync(userId);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                ApiResponse<IEnumerable<NavMenuItemDto>> response = new ApiResponse<IEnumerable<NavMenuItemDto>>
                {
                    Success = false,
                    Message = "Error fetching menu items",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<NavMenuItemDto>>> PostMenuItem(NavMenuItemDto menuItem)
        {
            try
            {

                var result = await _repo.CreateAsync(menuItem);

                return Ok(result);

            }
            catch (Exception ex)
            {
                ApiResponse<NavMenuItemDto> response = new ApiResponse<NavMenuItemDto>
                {
                    Success = false,
                    Message = "Error creating menu item",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };

                return BadRequest(response);

            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<NavMenuItemDto>>> PuttMenuItem(NavMenuItemDto menuItem)
        {
            try
            {

                var result = await _repo.UpdateAsync(menuItem);

                return Ok(result);

            }
            catch (Exception ex)
            {

                ApiResponse<NavMenuItemDto> response = new ApiResponse<NavMenuItemDto>
                {
                    Success = false,
                    Message = "Error creating menu item",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };

                return BadRequest(response);
            }
        }


    }
}
