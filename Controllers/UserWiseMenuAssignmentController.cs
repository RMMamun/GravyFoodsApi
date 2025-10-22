using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserWiseMenuAssignmentController : Controller
    {
        private readonly IUserWiseMenuItemRepository _repo;
        private readonly INavMenuRepository _menuRepo;

        public UserWiseMenuAssignmentController(IUserWiseMenuItemRepository repo, INavMenuRepository menuRepo)
        {
            _repo = repo;
            _menuRepo = menuRepo;
        }

        [HttpGet("{userId}/{companyId}/{branchId}")]
        public async Task<IActionResult> GetUserMenus(string userId, string companyId, string branchId)
        {
            var assignedMenus = await _repo.GetUserMenusAsync(userId, companyId, branchId);
            var allMenus = await _menuRepo.GetHierarchicalMenusAsync(companyId, branchId);

            var assignedIds = assignedMenus.Select(x => x.MenuId).ToList();
            return Ok(new { AllMenus = allMenus, AssignedIds = assignedIds });
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignMenus(UserMenuAssignmentRequest request)
        {
            await _repo.AssignMenusAsync(request);
            return Ok(new { Message = "Menu assignment updated successfully" });
        }



    }
}

