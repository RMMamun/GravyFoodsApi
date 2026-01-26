using GravyFoodsApi.Common;
using GravyFoodsApi.Data;
using GravyFoodsApi.DTO;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;


namespace GravyFoodsApi.MasjidServices
{
    public class UserInfoService : IUserInfoService
    {
        CommonMethods commonMethods = new CommonMethods();
        private readonly MasjidDBContext _dbContext;
        private readonly ITenantContextRepository _tenant;

        public UserInfoService(MasjidDBContext dbContext, ITenantContextRepository tenant)
        {
            _dbContext = dbContext;
            _tenant = tenant;
        }

        public async Task<long> GetNextUserIDAsync()
        {
            return await NextUserID();
        }

        private async Task<long> NextUserID()
        {
            var maxId = await _dbContext.UserInfo.Where(w => w.BranchId == _tenant.BranchId && w.CompanyId == _tenant.CompanyId).MaxAsync(u => u.Id);
            maxId = maxId == 0 ? 1 : maxId + 1;
            return maxId;
        }

        public async Task<UserInfo?> GetUserById(string userid)
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


                var result = await _dbContext.UserInfo.Where(w => w.UserId == userid && w.BranchId == _tenant.BranchId && w.CompanyId == _tenant.CompanyId).FirstOrDefaultAsync();
                return result;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<UserBasicInfoDTO>> GetAllUsersAsync(string branchId, string companyId)
        {

            try
            {
                List<UserBasicInfoDTO> result = await _dbContext.UserInfo
                    .Where(w => w.BranchId == _tenant.BranchId && w.CompanyId == _tenant.CompanyId)
                    .Select(user => new UserBasicInfoDTO
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        UserRole = user.UserRole,
                        EntryDateTime = user.EntryDateTime,
                        BranchId = user.BranchId,
                        CompanyId = user.CompanyId
                    })
                    .ToListAsync();
                return result;

            }
            catch (Exception ex)
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

                var isExisted = await GetUserById(user.UserId);
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


        //public async Task<bool> Update_AssignMasjidToUser(UserAsMasjidAdminDto userInfo)
        //{
        //    try
        //    {
        //        //UserInfoDTO user = new UserInfoDTO();
        //        var user = await _dbContext.UserInfo.Where(u => u.UserId == userInfo.UserId && u.DeviceId == userInfo.DeviceId).FirstOrDefaultAsync();
        //        if (user == null)
        //        {
        //            return false;
        //        }
        //        //else
        //        //{
        //        //    user.UserId = result.UserId;
        //        //    user.Password = result.Password;
        //        //    user.UserName = result.UserName;
        //        //    user.UserRole = result.UserRole;
        //        //    user.DeviceId = result.DeviceId;
        //        //    user.MasjidID = result.MasjidID;
        //        //    user.Latitude = result.Latitude;
        //        //    user.Longitude = result.Longitude;
        //        //    user.EntryDateTime = result.EntryDateTime;

        //        //}

        //        if (!string.IsNullOrWhiteSpace(user.MasjidID) || user.MasjidID == "")
        //        {
        //            user.MasjidID = userInfo.MasjidID;
        //        }

        //        //await _dbContext.UserInfo.Update(user);
        //        await _dbContext.SaveChangesAsync();

        //        //_dbContext.Update(user);
        //        //_dbContext.SaveChangesAsync();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


    }
}
