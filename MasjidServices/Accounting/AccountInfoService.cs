using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public async Task<ApiResponse<bool>> UpdateAccountAsync(AccountInfoDto account)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> DeleteAccountAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<AccountInfoDto>> GetAccountByIdAsync(string AccountId)
        {
            ApiResponse<AccountInfoDto> apiRes = new ApiResponse<AccountInfoDto>();

            try
            {

                var query =
                from acc in _context.AccountInfo.Where(a => a.Id.ToString() == AccountId && a.CompanyId == _tenant.CompanyId && a.BranchId == _tenant.BranchId)

                select new AccountInfoDto
                {
                    Id = acc.Id.ToString(),
                    ACCode = acc.ACCode,
                    ACName = acc.ACName,
                    ACType = acc.ACType,
                    Description = acc.Description,
                    ParentACCode = acc.ParentACCode,
                    ParentName = null,
                    IsControlAccount = acc.IsControlAccount,
                    IsActive = acc.IsActive
                };


                apiRes.Success = true;
                apiRes.Data = await query.FirstOrDefaultAsync();
                apiRes.Message = apiRes.Data != null ? "Accounts fetched successfully." : "No accounts found matching the search criteria.";

                return apiRes;


            }
            catch (Exception ex)
            {

                apiRes.Success = false;
                apiRes.Message = "An error occurred while fetching parent accounts.";
                apiRes.Errors = new List<string> { ex.Message };

                return apiRes;
            }
        }

        public async Task<ApiResponse<List<AccountInfoDto>>> GetAllAccountsAsync()
        {
            throw new NotImplementedException();
        }



        public async Task<ApiResponse<List<AccountInfoDto>>> GetParentAccountsAsync()
        {
            ApiResponse<List<AccountInfoDto>> apiRes = new ApiResponse<List<AccountInfoDto>>();
            try
            {
                
                var parentAccounts = await _context.AccountInfo
                    .Where(a => a.CompanyId == _tenant.CompanyId & a.BranchId == _tenant.BranchId)
                    .Select(a => new AccountInfoDto
                    {
                        Id = a.Id.ToString(),
                        ACCode = a.ACCode,
                        ACName = a.ACName,
                        ACType = a.ACType,
                        Description = a.Description,
                        ParentACCode = a.ParentACCode,
                        IsControlAccount = a.IsControlAccount,
                        IsActive = a.IsActive
                    })
                    .ToListAsync();


                apiRes.Success = true;
                apiRes.Data = parentAccounts;

                return apiRes;


            }
            catch (Exception ex)
            {

                apiRes.Success = false;
                apiRes.Message = "An error occurred while fetching parent accounts.";
                apiRes.Errors = new List<string> { ex.Message };
                
                return apiRes;
            }
        }

        public async Task<ApiResponse<List<AccountInfoDto>>> SearchAccountsAsync(string strSearch)
        {

            ApiResponse<List<AccountInfoDto>> apiRes = new ApiResponse<List<AccountInfoDto>>();

            try
            {
                if (strSearch == "ALL")
                {
                    strSearch = "";
                }

                //var query = _context.AccountInfo
                //.Select(acc => new AccountInfoDto
                //{
                //    Id = acc.Id.ToString(),
                //    ACCode = acc.ACCode,
                //    ACName = acc.ACName,
                //    ACType = acc.ACType,
                //    Description = acc.Description,
                //    ParentACCode = acc.ParentACCode,
                //    ParentName = acc.Parent.ACName,
                //    IsControlAccount = acc.IsControlAccount,
                //    IsActive = acc.IsActive
                //});

                //if (query != null)
                //{
                //    if (string.IsNullOrEmpty(AccountId) == false)
                //    {

                //        if (!string.IsNullOrWhiteSpace(AccountId))
                //        {
                //            query = query.Where(x =>
                //                x.ACName.Contains(AccountId) ||
                //                x.ACCode.Contains(AccountId) ||
                //                x.ParentName.Contains(AccountId));
                //        }
                //    }
                //}

                var query =
                from acc in _context.AccountInfo
                join parent in _context.AccountInfo
                    on acc.ParentACCode equals parent.ACCode into parentJoin
                from parent in parentJoin.DefaultIfEmpty()
                select new AccountInfoDto
                {
                    Id = acc.Id.ToString(),
                    ACCode = acc.ACCode,
                    ACName = acc.ACName,
                    ACType = acc.ACType,
                    Description = acc.Description,
                    ParentACCode = acc.ParentACCode,
                    ParentName = parent != null ? parent.ACName : null,
                    IsControlAccount = acc.IsControlAccount,
                    IsActive = acc.IsActive
                };


                if (!string.IsNullOrWhiteSpace(strSearch))
                {
                    query = query.Where(x =>
                        x.ACName.Contains(strSearch) ||
                        x.ACCode.Contains(strSearch) ||
                        (x.ParentName != null && x.ParentName.Contains(strSearch)));
                }

                apiRes.Success = true;
                apiRes.Data = await query.ToListAsync();
                apiRes.Message = apiRes.Data.Count > 0 ? "Accounts fetched successfully." : "No accounts found matching the search criteria.";

                return apiRes;


            }
            catch (Exception ex)
            {

                apiRes.Success = false;
                apiRes.Message = "An error occurred while fetching parent accounts.";
                apiRes.Errors = new List<string> { ex.Message };

                return apiRes;
            }
        }
    }
}
