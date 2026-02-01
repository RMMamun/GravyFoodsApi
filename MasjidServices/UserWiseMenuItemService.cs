using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using System;

using Microsoft.EntityFrameworkCore;
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;


namespace GravyFoodsApi.MasjidServices;

public class UserWiseMenuItemService : IUserWiseMenuItemRepository
{
    private readonly MasjidDBContext _context;
    private readonly ITenantContextRepository _tenant;

    public UserWiseMenuItemService(MasjidDBContext context, ITenantContextRepository tenant) 
    {
        _context = context;
        _tenant = tenant;
    }

    public async Task<ApiResponse<List<UserWiseMenuAssignment>>> GetUserMenusAsync(string userId)
    {
        ApiResponse<List<UserWiseMenuAssignment>> apiRes = new ();
        try
        {

            var result = await _context.UserWiseMenuAssignment
                .Include(x => x.Menu)
                .Where(x => x.UserId == userId && x.CompanyId == _tenant.CompanyId && x.BranchId == _tenant.BranchId)
                .ToListAsync();

            apiRes.Success = true;
            apiRes.Message = "User menus fetched successfully";
            apiRes.Data = result;

            return apiRes;
        }
        catch (Exception ex)
        {

            apiRes.Success = false;
            apiRes.Message = $"Error fetching user menus: {ex.Message}";
            apiRes.Data = null;
            apiRes.Errors = new List<string> { ex.Message };

            return apiRes;
        }
    }

    public async Task AssignMenusAsync(UserMenuAssignmentRequest request)
    {
        var existing = _context.UserWiseMenuAssignment
            .Where(x => x.UserId == request.UserId && x.CompanyId == _tenant.CompanyId && x.BranchId == _tenant.BranchId);

        _context.UserWiseMenuAssignment.RemoveRange(existing);

        foreach (var menuId in request.MenuIds.Distinct())
        {
            _context.UserWiseMenuAssignment.Add(new UserWiseMenuAssignment
            {
                UserId = request.UserId,
                MenuId = menuId,
                CompanyId = _tenant.CompanyId,
                BranchId = _tenant.BranchId
            });
        }

        await _context.SaveChangesAsync();
    }


    



}
