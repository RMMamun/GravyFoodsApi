using MasjidApi.Common;
using MasjidApi.Data;
using MasjidApi.DTO;
using MasjidApi.MasjidRepository;
using MasjidApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace MasjidApi.MasjidServices
{
    public class UserInfoService : IUserInfoService
    {
        CommonMethods commonMethods = new CommonMethods();
        private readonly MasjidDBContext _dbContext;
        public UserInfoService(MasjidDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> GetNextUserIDAsync()
        {
            return NextUserID();
        }

        private long NextUserID()
        {
            var maxId = _dbContext.UserInfo.Max(u => u.Id);
            maxId = maxId == 0 ? 1 : maxId + 1;
            return maxId;
        }

        public async Task<UserInfo> GetUserById(string userid)
        {

            try
            {
                //// Create a logger factory
                //var loggerFactory = LoggerFactory.Create(builder =>
                //{
                //    builder
                //        .AddFilter((category, level) =>
                //            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                //        .AddConsole();
                //});

                //// Configure DbContext to use the logger factory
                //_dbContext.Database.SetCommandLoggerFactory(loggerFactory);


                var result = await _dbContext.UserInfo.Where(x => x.UserId == userid).FirstOrDefaultAsync();
                return result;

            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public async Task<UserInfo> Create(UserInfo user)
        {
            try
            {
                if (user.UserName == "@N*e^w?U/s>e$r#")
                {
                    var maxId = NextUserID();

                    user.UserId = "User" + maxId.ToString();
                    user.UserName = user.UserId;

                    const string chars1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    const string chars2 = "0123456789!@#$%^&*()ABCDEFGHIJKL0123456789!@#$%^&*()MNOPQRSTUVWXY0123456789!@#$%^&*()Z0123456789!@#$%^&*()abcdefghijklmnop0123456789!@#$%^&*()qrstuvwxyz0123456789!@#$%^&*()";
                    Random random = new Random();

                    var Password = new string(Enumerable.Repeat(chars2, 12).Select(s => s[random.Next(s.Length)]).ToArray());

                    user.Password = Password;    //commonMethods.GenerateRandomString(10);
                }

                var isExisted = await _dbContext.UserInfo.Where(x => x.UserId == user.UserId).FirstOrDefaultAsync();
                if (isExisted == null)
                {
                    await _dbContext.UserInfo.AddAsync(user);
                }
                else
                {
                    //_dbContext.UserInfo.Update(user);
                    _dbContext.Entry(isExisted).CurrentValues.SetValues(user);

                }
                
                await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<bool> Update_AssignMasjidToUser(UserAsMasjidAdminDto userInfo)
        {
            try
            {
                //UserInfoDTO user = new UserInfoDTO();
                var user = await _dbContext.UserInfo.Where(u => u.UserId == userInfo.UserId && u.DeviceId == userInfo.DeviceId).FirstOrDefaultAsync();
                if (user == null)
                {
                    return false;
                }
                //else
                //{
                //    user.UserId = result.UserId;
                //    user.Password = result.Password;
                //    user.UserName = result.UserName;
                //    user.UserRole = result.UserRole;
                //    user.DeviceId = result.DeviceId;
                //    user.MasjidID = result.MasjidID;
                //    user.Latitude = result.Latitude;
                //    user.Longitude = result.Longitude;
                //    user.EntryDateTime = result.EntryDateTime;

                //}

                if (!string.IsNullOrWhiteSpace(user.MasjidID) || user.MasjidID == "")
                {
                    user.MasjidID = userInfo.MasjidID;
                }

                //await _dbContext.UserInfo.Update(user);
                await _dbContext.SaveChangesAsync();

                //_dbContext.Update(user);
                //_dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
