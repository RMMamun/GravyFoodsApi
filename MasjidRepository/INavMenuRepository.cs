using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface INavMenuRepository
    {
        Task<List<NavMenuItem>> GetAllMenusAsync(string companyId, string branchId);
        Task<List<NavMenuItemDto>> GetHierarchicalMenusAsync(string companyId, string branchId);

        Task<List<NavMenuItemDto>> GetParentMenusAsync(string companyId, string branchId);

        Task<NavMenuItem> CreateAsync(NavMenuItem navMenuItem);
        Task<NavMenuItem> UpdateAsync(NavMenuItem menuItem);

        Task<IEnumerable<NavMenuItemDto>> GetMenusByUserAsync(string userId, string companyId, string branchId);
    }

}
