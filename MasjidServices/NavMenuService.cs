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


        //Copilot Suggested Implementation
        public async Task<IEnumerable<NavMenuItemDto>> GetMenusByUserAsync(string userId, string companyId, string branchId)
        {
            try
            {
                // Query menus by joining to UserWiseMenuAssignment so EF emits a JOIN (no OPENJSON)
                var menus = await (from m in _context.NavMenuItems
                                   join a in _context.UserWiseMenuAssignment
                                      on new { m.MenuId, m.CompanyId, m.BranchId } equals new { MenuId = a.MenuId, a.CompanyId, a.BranchId }
                                   where a.UserId == userId
                                         && a.CompanyId == companyId
                                         && a.BranchId == branchId
                                         && m.IsActive
                                   orderby m.DisplayOrder
                                   select m)
                                  .AsNoTracking()
                                  .ToListAsync();

                if (menus == null || menus.Count == 0)
                    return Enumerable.Empty<NavMenuItemDto>();

                var menuLookup = menus.ToLookup(m => m.ParentId);

                List<NavMenuItemDto> BuildHierarchy(int? parentId)
                {
                    return menuLookup[parentId]
                        .Select(m => new NavMenuItemDto
                        {
                            MenuId = m.MenuId,
                            Title = m.Title,
                            Url = m.Url,
                            IconCss = m.IconCss,
                            IconImagePath = m.IconImagePath,
                            IsSeparator = m.IsSeparator,
                            CompanyId = m.CompanyId,
                            BranchId = m.BranchId,
                            Children = BuildHierarchy(m.MenuId)
                        })
                        .OrderBy(x => menus.FirstOrDefault(mm => mm.MenuId == x.MenuId)?.DisplayOrder ?? 0)
                        .ToList();
                }

                var parentMenus = BuildHierarchy(null);
                return parentMenus;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menus for user '{userId}': {ex.Message}", ex);
            }
        }



        //Chat GPT Suggested Implementation - commented out for reference
        //public async Task<IEnumerable<NavMenuItemDto>> GetMenusByUserAsync(string userId, string companyId, string branchId)
        //{
        //    try
        //    {
        //        // Step 1: Load assigned MenuIds
        //        var assignedMenuIds = await _context.UserWiseMenuAssignment
        //            .Where(a => a.UserId == userId && a.CompanyId == companyId && a.BranchId == branchId)
        //            .Select(a => a.MenuId)
        //            .ToListAsync();

        //        // Safety: avoid empty list causing SQL JSON translation
        //        if (assignedMenuIds == null || !assignedMenuIds.Any())
        //            return Enumerable.Empty<NavMenuItemDto>();

        //        // Step 2: Force EF to generate simple IN clause
        //        var menus = await _context.NavMenuItems
        //            .Where(m => assignedMenuIds.Contains(m.MenuId))
        //            .AsEnumerable() // <— force LINQ-to-Objects after this
        //            .Where(m => m.IsActive) // <— filtering in memory
        //            .OrderBy(m => m.DisplayOrder)
        //            .ToListAsyncSafe(); // custom extension below


        //        // Step 3: Build hierarchy
        //        var parentMenus = menus
        //            .Where(m => m.ParentId == null)
        //            .Select(m => new NavMenuItemDto
        //            {
        //                MenuId = m.MenuId,
        //                Title = m.Title,
        //                Url = m.Url,
        //                IconCss = m.IconCss,
        //                IconImagePath = m.IconImagePath,
        //                IsSeparator = m.IsSeparator,
        //                CompanyId = m.CompanyId,
        //                BranchId = m.BranchId,
        //                Children = menus
        //                    .Where(c => c.ParentId == m.MenuId)
        //                    .OrderBy(c => c.DisplayOrder)
        //                    .Select(c => new NavMenuItemDto
        //                    {
        //                        MenuId = c.MenuId,
        //                        Title = c.Title,
        //                        Url = c.Url,
        //                        IconCss = c.IconCss,
        //                        IconImagePath = c.IconImagePath,
        //                        IsSeparator = c.IsSeparator,
        //                        CompanyId = c.CompanyId,
        //                        BranchId = c.BranchId
        //                    }).ToList()
        //            }).ToList();

        //        return parentMenus;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error retrieving menus for user: {ex.Message}", ex);
        //    }
        //}


    }


    public static class AsyncHelpers
    {
        public static Task<List<T>> ToListAsyncSafe<T>(this IEnumerable<T> source)
        {
            return Task.FromResult(source.ToList());
        }
    }

}
