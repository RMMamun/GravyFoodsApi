using GravyFoodsApi.Common;
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class LoginService : ILoginService
    {
      
        private readonly MasjidDBContext _dbContext;
        private readonly IUserInfoService _userInfoService;
        public LoginService(MasjidDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<UserInfoDTO?> GetUser(string userId, string password, string branchId, string companyId)
        {
            try
            {
                //var str = GlobalVariable.ConnString;

                var user = await _dbContext.UserInfo.Where(u =>
                    u.UserId == userId &&
                    u.Password == password &&
                    u.BranchId == branchId &&
                    u.CompanyId == companyId)
                    .Select(u => new UserInfoDTO
                    {
                        UserId = u.UserId,
                        UserName = u.UserName,
                        Password = u.Password,
                        BranchId = u.BranchId,
                        CompanyId = u.CompanyId,
                        DeviceId = u.DeviceId,
                        MasjidID = u.MasjidID,
                        UserRole = u.UserRole,
                        Email = u.Email,
                        EntryDateTime = u.EntryDateTime,
                        Latitude = u.Latitude,
                        Longitude = u.Longitude,
                        


                    })
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return null;
            }

        }
    }
}
