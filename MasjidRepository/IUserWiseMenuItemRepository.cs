using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IUserWiseMenuItemRepository
    {
        Task<ApiResponse<List<UserWiseMenuAssignment>>> GetUserMenusAsync(string userId);
        Task AssignMenusAsync(UserMenuAssignmentRequest request);



    }
}
