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


            

        [HttpGet("{companyId}/{branchId}")]
        public async Task<IActionResult> GetHierarchicalMenus(string companyId, string branchId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            companyId = _tenant.CompanyId;     //User.FindFirstValue("companyId");
            branchId = _tenant.BranchId;        //User.FindFirstValue("branchId");

            var menus = await _repo.GetHierarchicalMenusAsync(companyId, branchId);
            return Ok(menus);
        }

        [HttpGet("parents/{companyId}/{branchId}")]
        public async Task<ActionResult<IEnumerable<NavMenuItemDto>>> GetParentMenus(string companyId, string branchId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            companyId = _tenant.CompanyId;     //User.FindFirstValue("companyId");
            branchId = _tenant.BranchId;        //User.FindFirstValue("branchId");

            var parents = await _repo.GetParentMenusAsync(companyId, branchId);

            return Ok(parents);
        }

        [HttpGet("menuitems/{userId}/{companyId}/{branchId}")]
        public async Task<ActionResult<IEnumerable<NavMenuItemDto>>> GetMenuItems(string userId, string companyId, string branchId)
        {
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            companyId = _tenant.CompanyId;     //User.FindFirstValue("companyId");
            branchId = _tenant.BranchId;        //User.FindFirstValue("branchId");

            var menus = await _repo.GetMenusByUserAsync(userId, companyId, branchId);
            return Ok(menus);
        }

        [HttpPost]
        public async Task<ActionResult<NavMenuItem>> PostMenuItem(NavMenuItem menuItem)
        {
            try
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var companyId = _tenant.CompanyId;     //User.FindFirstValue("companyId");
                var branchId = _tenant.BranchId;        //User.FindFirstValue("branchId");

                menuItem.BranchId = branchId;
                menuItem.CompanyId = companyId;


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

        [HttpPut]
        public async Task<ActionResult<NavMenuItem>> PuttMenuItem(NavMenuItem menuItem)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var companyId = _tenant.CompanyId;     //User.FindFirstValue("companyId");
                var branchId = _tenant.BranchId;        //User.FindFirstValue("branchId");

                menuItem.BranchId = branchId;
                menuItem.CompanyId = companyId;


                var result = await _repo.UpdateAsync(menuItem);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
