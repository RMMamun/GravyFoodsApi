using MasjidApi.DTO;
using MasjidApi.Models;

namespace MasjidApi.MasjidRepository
{
    public interface IUserInfoService
    {
        Task<long> GetNextUserIDAsync();

        Task<UserInfo> GetUserById(string userid);
        Task<UserInfo> Create(UserInfo userInfo);
        //Task<UserInfo> Update(UserInfo userInfo);

        Task<bool> Update_AssignMasjidToUser(UserAsMasjidAdminDto userInfo);

    }
}
