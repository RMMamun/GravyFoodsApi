using GravyFoodsApi.Common;
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;

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
        
        public LoginRequest? GetUser(string username, string password)
        {
            try
            {
                var str = GlobalVariable.ConnString;

                var user = _dbContext.UserInfo.Where(u =>
                    u.UserId.Equals(username) &&
                    u.Password == password)
                    .Select(u => new LoginRequest
                    {
                        Username = u.UserId,
                        Password = u.Password
                    })
                    .FirstOrDefault();

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
