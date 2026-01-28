using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface INavMenuRepository
    {
        Task<ApiResponse<List<NavMenuItemDto>>> GetAllMenusAsync();
        Task<ApiResponse<List<NavMenuItemDto>>> GetHierarchicalMenusAsync();

        Task<ApiResponse<List<NavMenuItemDto>>> GetParentMenusAsync();

        Task<ApiResponse<NavMenuItemDto>> CreateAsync(NavMenuItemDto navMenuItem);
        Task<ApiResponse<NavMenuItemDto>> UpdateAsync(NavMenuItemDto menuItem);

        Task<ApiResponse<IEnumerable<NavMenuItemDto>>> GetMenusByUserAsync(string userId);
    }

}
