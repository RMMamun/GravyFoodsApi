using GravyFoodsApi.Models.Accounting;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.MasjidRepository.Accounting
{
    public interface IAccountInfoRepository
    {
        Task<ApiResponse<bool>> CreateAccountAsync(AccountInfoDto account);
        Task<ApiResponse<AccountInfoDto>> GetAccountByIdAsync(string AccountId);
        Task<ApiResponse<List<AccountInfoDto>>> GetAllAccountsAsync();
        Task<ApiResponse<List<AccountInfoDto>>> SearchAccountsAsync(string strSearch);
        Task<ApiResponse<List<AccountInfoDto>>> GetParentAccountsAsync();
        Task<ApiResponse<bool>> UpdateAccountAsync(AccountInfoDto account);
        Task<ApiResponse<bool>> DeleteAccountAsync(Guid id);
    }
}
