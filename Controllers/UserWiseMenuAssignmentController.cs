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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserMenus(string userId)
        {
            try
            {
                var assignedMenus = await _repo.GetUserMenusAsync(userId);
                var allMenus = await _menuRepo.GetHierarchicalMenusAsync();

                if (assignedMenus.Data != null && allMenus.Data != null)
                {
                    var assignedIds = assignedMenus.Data.Select(x => x.MenuId).ToList();
                    return Ok(new { AllMenus = allMenus, AssignedIds = assignedIds });
                }
                return Ok(new { AllMenus = allMenus, AssignedIds = new List<int>() });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = $"Error fetching user menus: {ex.Message}" });
            }
        }


        [HttpPost("assign")]
        public async Task<IActionResult> AssignMenus(UserMenuAssignmentRequest request)
        {
            await _repo.AssignMenusAsync(request);
            return Ok(new { Message = "Menu assignment updated successfully" });
        }



    }
}

