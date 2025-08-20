using GravyFoodsApi.DTO;
using GravyFoodsApi.Models;

namespace GravyFoodsApi.MasjidRepository
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
