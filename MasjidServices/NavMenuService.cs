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
        private readonly ITenantContextRepository _tenant;
        public NavMenuService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<ApiResponse<List<NavMenuItemDto>>> GetAllMenusAsync()
        {
            ApiResponse<List<NavMenuItemDto>> apiRes = new ApiResponse<List<NavMenuItemDto>>();
            try
            {
                var menus = await _context.NavMenuItems
                    .Where(x => x.CompanyId == _tenant.CompanyId && x.BranchId == _tenant.BranchId)
                    .OrderBy(x => x.DisplayOrder)
                    .ToListAsync();
                
                if (menus.Count == 0) {
                    apiRes.Success = false;
                    apiRes.Message = "No menu items found.";
                    return apiRes;
                }

                var menuDtos = menus.Select(m => new NavMenuItemDto
                {
                    MenuId = m.MenuId,
                    Title = m.Title,
                    Url = m.Url,
                    IconCss = m.IconCss,
                    IconImagePath = m.IconImagePath,
                    IsSeparator = m.IsSeparator,
                    DisplayOrder = m.DisplayOrder,
                    IsActive = m.IsActive,
                    ParentId = m.ParentId,
                    
                }).ToList();
                
                apiRes.Success = true;
                apiRes.Data = menuDtos;
                apiRes.Message = "Menu items retrieved successfully.";

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error retrieving menu items.";
                apiRes.Errors = new List<string> { ex.Message };
                return apiRes;
            }

        }

        public async Task<ApiResponse<List<NavMenuItemDto>>> GetHierarchicalMenusAsync()
        {
            ApiResponse<List<NavMenuItemDto>> apiRes = new ApiResponse<List<NavMenuItemDto>>();

            try
            {
                var items = await GetAllMenusAsync();
                if (items.Data == null)
                {
                    apiRes.Success = false;
                    apiRes.Message = "No menu items found.";

                    return apiRes;
                }

                List<NavMenuItemDto> ToDtoTree(int? parentId)
                {
                    return items.Data
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
                            DisplayOrder = x.DisplayOrder,
                            IsActive = x.IsActive,
                            ParentId = x.ParentId,
                            Children = ToDtoTree(x.MenuId),

                        }).ToList();
                }

                apiRes.Success = true;
                apiRes.Message = "Hierarchical menu items retrieved successfully.";
                apiRes.Data = ToDtoTree(null);

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error retrieving hierarchical menu items.";
                apiRes.Errors = new List<string> { ex.Message };
                return apiRes;
            }
        }

        public async Task<ApiResponse<List<NavMenuItemDto>>> GetParentMenusAsync()
        {
            ApiResponse<List<NavMenuItemDto>> apiRes = new ApiResponse<List<NavMenuItemDto>>();

            try
            {

                //Initial design was to get only parent menus, but changed to get all menus
                //.Where(m => m.ParentACCode == null && m.BranchId == branchId && m.CompanyId == companyId)

                var parents = await _context.NavMenuItems
                .Where(m => m.BranchId == _tenant.BranchId && m.CompanyId == _tenant.CompanyId)
                .Select(m => new NavMenuItemDto
                {
                    MenuId = m.MenuId,
                    Title = m.Title,
                    DisplayOrder = m.DisplayOrder,
                    IconCss = m.IconCss,
                    IconImagePath = m.IconImagePath,
                    IsActive = m.IsActive,
                    IsSeparator = m.IsSeparator,
                    Url = m.Url,
                    ParentId = m.ParentId

                })
                .ToListAsync();


                apiRes.Data = parents;
                apiRes.Success = true;
                apiRes.Message = "Parent menus retrieved successfully.";

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error retrieving parent menus.";
                apiRes.Errors = new List<string> { ex.Message };
                return apiRes;
            }

        }

        public async Task<ApiResponse<NavMenuItemDto>> CreateAsync(NavMenuItemDto menuItem)
        {
            ApiResponse<NavMenuItemDto> apiRes = new ();

            string _branchId = _tenant.BranchId;
            string _companyId = _tenant.CompanyId;

            try
            {
                //check the existence
                var exists = await _context.NavMenuItems
                    .AnyAsync(m => m.Title == menuItem.Title
                    && m.CompanyId == _companyId
                    && m.BranchId == _branchId);

                if (exists)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Menu with the same title already exists.";

                    return apiRes;
                    
                }

                //create menuid as last menuid + 1
                //var lastMenu = await _context.NavMenuItems
                //    .Where(m => m.CompanyId == _companyId && m.BranchId == _branchId)
                //    .OrderByDescending(m => m.MenuId)
                //    .FirstOrDefaultAsync();

                var maxId = await _context.NavMenuItems
                    .MaxAsync(m => (int?)m.MenuId) ?? 0;

                menuItem.MenuId = maxId + 1;

                var newMenus = new NavMenuItem
                {
                    MenuId = menuItem.MenuId,
                    ParentId = menuItem.ParentId,
                    Title = menuItem.Title,
                    Url = menuItem.Url,
                    IconCss = menuItem.IconCss,
                    IconImagePath = menuItem.IconImagePath,
                    DisplayOrder = menuItem.DisplayOrder,
                    IsActive = menuItem.IsActive,
                    IsSeparator = menuItem.IsSeparator,
                    CompanyId = _companyId,
                    BranchId = _branchId
                };

                _context.NavMenuItems.Add(newMenus);
                await _context.SaveChangesAsync();


                var createdItem = await _context.NavMenuItems
                    .FirstOrDefaultAsync(m => m.Title == menuItem.Title
                    && m.CompanyId == _companyId
                    && m.BranchId == _branchId);
                
                

                if (createdItem != null)
                {
                    var createdMenuDto = new NavMenuItemDto
                    {
                        MenuId = createdItem.MenuId,
                        ParentId = createdItem.ParentId,
                        Title = createdItem.Title,
                        Url = createdItem.Url,
                        IconCss = createdItem.IconCss,
                        IconImagePath = createdItem.IconImagePath,
                        DisplayOrder = createdItem.DisplayOrder,
                        IsActive = createdItem.IsActive,
                        IsSeparator = createdItem.IsSeparator,
                        
                    };

                    apiRes.Success = true;
                    apiRes.Message = "Menu item created successfully.";
                    apiRes.Data = createdMenuDto;
                }
                else
                {
                    apiRes.Success = false;
                    apiRes.Message = "Error retrieving the created menu item.";
                    apiRes.Data = null;
                }

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error creating menu item." + ex.Message;

                return apiRes;
            }
        }


        public async Task<ApiResponse<NavMenuItemDto>> UpdateAsync(NavMenuItemDto menuItem)
        {
            ApiResponse<NavMenuItemDto> apiRes = new();

            string _branchId = _tenant.BranchId;
            string _companyId = _tenant.CompanyId;

            try
            {
                //check the existence
                var exists = await _context.NavMenuItems
                    .AnyAsync(m => m.MenuId == menuItem.MenuId
                    && m.CompanyId == _companyId
                    && m.BranchId == _branchId);

                if (exists == false)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Menu item does not exist.";

                    return apiRes;

                }

                var updated = new NavMenuItem
                    {
                    MenuId = menuItem.MenuId,
                    ParentId = menuItem.ParentId,
                    Title = menuItem.Title,
                    Url = menuItem.Url,
                    IconCss = menuItem.IconCss,
                    IconImagePath = menuItem.IconImagePath,
                    DisplayOrder = menuItem.DisplayOrder,
                    IsActive = menuItem.IsActive,
                    IsSeparator = menuItem.IsSeparator,
                    CompanyId = _companyId,
                    BranchId = _branchId
                };


                _context.NavMenuItems.Update(updated);
                await _context.SaveChangesAsync();


                apiRes.Success = true;
                apiRes.Message = "Menu item updated successfully.";
                apiRes.Data = menuItem;

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error updating menu item." + ex.Message;

                return apiRes;
            }
        }


        //Copilot Suggested Implementation
        public async Task<ApiResponse<IEnumerable<NavMenuItemDto>>> GetMenusByUserAsync(string userId)
        {
            string _branchId = _tenant.BranchId;
            string _companyId = _tenant.CompanyId;

            ApiResponse<IEnumerable<NavMenuItemDto>> apiRes = new ();

            try
            {
                // Query menus by joining to UserWiseMenuAssignment so EF emits a JOIN (no OPENJSON)
                var menus = await (from m in _context.NavMenuItems
                                   join a in _context.UserWiseMenuAssignment
                                      on new { m.MenuId, m.CompanyId, m.BranchId } equals new { MenuId = a.MenuId, a.CompanyId, a.BranchId }
                                   where a.UserId == userId
                                         && a.CompanyId == _companyId
                                         && a.BranchId == _branchId
                                         && m.IsActive
                                   orderby m.DisplayOrder
                                   select m)
                                  .AsNoTracking()
                                  .ToListAsync();

                if (menus == null || menus.Count == 0)
                {
                    apiRes.Success = false;
                    apiRes.Message = "No menu items found for the user.";

                    return apiRes;
                }

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
                            Children = BuildHierarchy(m.MenuId)
                        })
                        .OrderBy(x => menus.FirstOrDefault(mm => mm.MenuId == x.MenuId)?.DisplayOrder ?? 0)
                        .ToList();
                }

                var parentMenus = BuildHierarchy(null);
                apiRes.Data = parentMenus;
                apiRes.Success = true;
                apiRes.Message = "Menu items retrieved successfully for the user.";

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;

                return apiRes;
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
        //            .Where(m => m.ParentACCode == null)
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
        //                    .Where(c => c.ParentACCode == m.MenuId)
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
