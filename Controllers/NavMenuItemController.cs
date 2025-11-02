using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidServices;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class NavMenuItemController : Controller
    {
        private readonly INavMenuRepository _repo;
        public NavMenuItemController(INavMenuRepository repo) => _repo = repo;

        [HttpGet("{companyId}/{branchId}")]
        public async Task<IActionResult> GetHierarchicalMenus(string companyId, string branchId)
        {
            var menus = await _repo.GetHierarchicalMenusAsync(companyId, branchId);
            return Ok(menus);
        }

        [HttpGet("parents/{companyId}/{branchId}")]
        public async Task<ActionResult<IEnumerable<NavMenuItemDto>>> GetParentMenus(string companyId, string branchId)
        {

            var parents = await _repo.GetParentMenusAsync(companyId, branchId);

            return Ok(parents);
        }

        [HttpGet("menuitems/{userId}/{companyId}/{branchId}")]
        public async Task<ActionResult<IEnumerable<NavMenuItemDto>>> GetMenuItems(string userId, string companyId, string branchId)
        {
            var menus = await _repo.GetMenusByUserAsync(userId, companyId, branchId);
            return Ok(menus);
        }

        [HttpPost]
        public async Task<ActionResult<NavMenuItem>> PostMenuItem(NavMenuItem menuItem)
        {

            //var menus = await _repo.CreateAsync(menuItem);

            //return Ok(menus);

            try
            {

                var result = await _repo.CreateAsync(menuItem);

                return Ok(result);

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already exists"))
                    return Conflict(new { message = ex.Message }); // HTTP 409 Conflict

                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
