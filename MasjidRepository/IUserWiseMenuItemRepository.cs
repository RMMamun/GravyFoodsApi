using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IUserWiseMenuItemRepository
    {
        Task<List<UserWiseMenuAssignment>> GetUserMenusAsync(string userId, string companyId, string branchId);
        Task AssignMenusAsync(UserMenuAssignmentRequest request);

    }
}
