using GravyFoodsApi.MasjidRepository;
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

        [HttpPost]
        public async Task<ActionResult<NavMenuItem>> PostMenuItem(NavMenuItem menuItem)
        {
            var menus = await _repo.CreateAsync(menuItem);

            return Ok(menus);
        }


    }
}
