using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.MasjidServices.Accounting
{
    public class AccountInfoService : IAccountInfoRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;
        public AccountInfoService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<ApiResponse<bool>> CreateAccountAsync(AccountInfoDto account)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> DeleteAccountAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<AccountInfoDto>> GetAccountByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<AccountInfoDto>>> GetAllAccountsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> UpdateAccountAsync(AccountInfoDto account)
        {
            throw new NotImplementedException();
        }
    }
}
