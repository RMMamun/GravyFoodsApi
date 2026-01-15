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
        
        public async Task<LoginRequest?> GetUser(string username, string password, string branchId, string companyId)
        {
            try
            {
                //var str = GlobalVariable.ConnString;

                var user = await _dbContext.UserInfo.Where(u =>
                    u.UserId.Equals(username) &&
                    u.Password == password &&
                    u.BranchId == branchId &&
                    u.CompanyId == companyId)
                    .Select(u => new LoginRequest
                    {
                        Username = u.UserId,
                        Password = u.Password
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
