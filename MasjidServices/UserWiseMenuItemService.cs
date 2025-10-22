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

    public UserWiseMenuItemService(MasjidDBContext context) 
    {
        _context = context;
    }

    public async Task<List<UserWiseMenuAssignment>> GetUserMenusAsync(string userId, string companyId, string branchId)
    {
        return await _context.UserWiseMenuAssignment
            .Include(x => x.Menu)
            .Where(x => x.UserId == userId && x.CompanyId == companyId && x.BranchId == branchId)
            .ToListAsync();
    }

    public async Task AssignMenusAsync(UserMenuAssignmentRequest request)
    {
        var existing = _context.UserWiseMenuAssignment
            .Where(x => x.UserId == request.UserId && x.CompanyId == request.CompanyId && x.BranchId == request.BranchId);

        _context.UserWiseMenuAssignment.RemoveRange(existing);

        foreach (var menuId in request.MenuIds.Distinct())
        {
            _context.UserWiseMenuAssignment.Add(new UserWiseMenuAssignment
            {
                UserId = request.UserId,
                MenuId = menuId,
                CompanyId = request.CompanyId,
                BranchId = request.BranchId
            });
        }

        await _context.SaveChangesAsync();
    }


    



}
