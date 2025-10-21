using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;

namespace GravyFoodsApi.MasjidServices
{
    public class NavMenuService : INavMenuRepository
    {
        private readonly MasjidDBContext _context;
        public NavMenuService(MasjidDBContext context) => _context = context;

        public async Task<List<NavMenuItem>> GetAllMenusAsync(string companyId, string branchId)
        {
            return await _context.NavMenuItems
                .Where(x => x.CompanyId == companyId && x.BranchId == branchId && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<NavMenuItemDto>> GetHierarchicalMenusAsync(string companyId, string branchId)
        {
            var items = await GetAllMenusAsync(companyId, branchId);

            List<NavMenuItemDto> ToDtoTree(int? parentId)
            {
                return items
                    .Where(x => x.ParentId == parentId)
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new NavMenuItemDto
                    {
                        MenuId = x.MenuId,
                        Title = x.Title,
                        Url = x.Url,
                        IconCss = x.IconCss,
                        IconImagePath = x.IconImagePath,
                        IsSeparator = x.IsSeparator,
                        BranchId = x.BranchId,
                        CompanyId = x.CompanyId,
                        Children = ToDtoTree(x.MenuId)
                    }).ToList();
            }

            return ToDtoTree(null);
        }


        public async Task<List<NavMenuItemDto>> GetParentMenusAsync(string companyId, string branchId)
        {
            var parents = await _context.NavMenuItems
            .Where(m => m.ParentId == null && m.BranchId == branchId && m.CompanyId == companyId)
            .Select(m => new NavMenuItemDto { MenuId = m.MenuId, Title = m.Title })
            .ToListAsync();



            return parents;
        }

        public async Task<NavMenuItem> CreateAsync(NavMenuItem menuItem)
        {
            try
            {
                //check the existence
                var exists = await _context.NavMenuItems
                    .AnyAsync(m => m.Title == menuItem.Title
                    && m.CompanyId == menuItem.CompanyId
                    && m.BranchId == menuItem.BranchId);

                if (exists)
                {
                    throw new Exception("Menu item with the same title already exists in this company and branch.");
                }

                //create menuid as last menuid + 1
                var lastMenu = await _context.NavMenuItems
                    .Where(m => m.CompanyId == menuItem.CompanyId && m.BranchId == menuItem.BranchId)
                    .OrderByDescending(m => m.MenuId)
                    .FirstOrDefaultAsync();

                menuItem.MenuId = lastMenu != null ? lastMenu.MenuId + 1 : 1;



                _context.NavMenuItems.Add(menuItem);
                await _context.SaveChangesAsync();


                var createdItem = await _context.NavMenuItems
                    .FirstOrDefaultAsync(m => m.Title == menuItem.Title
                    && m.CompanyId == menuItem.CompanyId
                    && m.BranchId == menuItem.BranchId);

                if (createdItem != null)
                {
                    return createdItem;
                }
                else
                {
                    throw new Exception("Error creating menu item.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating menu item: {ex.Message}");

            }
        }

    }

}
