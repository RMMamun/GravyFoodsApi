using GravyFoodsApi.Common;
using GravyFoodsApi.Data;
using GravyFoodsApi.DTO;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class SubscriptionInfoService : ISubscriptionInfo
    {

        CommonMethods commonMethods = new CommonMethods();
        private readonly MasjidDBContext _dbContext;
        private readonly IBranchInfoRepository _branchRep;

        public SubscriptionInfoService(MasjidDBContext dbContext, IBranchInfoRepository branchRep)
        {
            _dbContext = dbContext;
            _branchRep = branchRep;
        }

        public async Task<bool> isExisted(SubscriptionDto subsDto)
        {
            try
            {

                var result = await _dbContext.SubscriptionInfo.Where(x => x.DeviceKey == subsDto.DeviceKey).FirstOrDefaultAsync();

                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> CheckSubscription(SubscriptionDto subsDto)
        {
            try
            {

                var result = await _dbContext.SubscriptionInfo.Where(x => x.DeviceKey == subsDto.DeviceKey).FirstOrDefaultAsync();

                if (result != null)
                {
                    //int remainDays = checkCompany.SubscriptionEndDate - subsDto.SubscriptionEndDate;
                    // Step 2: Calculate the difference
                    TimeSpan difference = result.SubscriptionEndDate - subsDto.SubscriptionEndDate;

                    // Step 3: Extract the number of days from the TimeSpan
                    int daysDifference = difference.Days;

                    return daysDifference;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<SubscriptionInfo>?> GetAllAsync()
        {
            try
            {

                var result = await _dbContext.SubscriptionInfo.OrderBy(x => x.SubscriptionStartDate).ToListAsync();

                if (result != null)
                {
                    
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<SubscriptionInfo> Create(SubscriptionInfo subscription)
        {
            try
            {

                var result = await _dbContext.SubscriptionInfo.AddAsync(subscription);
                await _dbContext.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateSubscriptionAsync(SubscriptionInfo subs)
        {
            try
            {

                
                var result = await _dbContext.SubscriptionInfo.Where(x => x.DeviceKey == subs.DeviceKey).FirstOrDefaultAsync();
                if (result != null)
                {

                    result.SubscriptionEndDate = subs.SubscriptionEndDate;

                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }

                
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<ApiResponse<CompanyRegistrationResponseDto?>> GetCompanyRegistrationVerificationAsync(string RegCode)
        {

            ApiResponse<CompanyRegistrationResponseDto?> apiRes = new ApiResponse<CompanyRegistrationResponseDto?>();
            try
            {

                var checkReg = await _dbContext.SubscriptionInfo.FirstOrDefaultAsync(b => b.RegCode == RegCode);
                if (checkReg == null)
                {
                    //return null;
                    apiRes.Success = false;
                    apiRes.Message = "Invalid Registration Code.";
                    apiRes.Data = null;

                    return apiRes;
                }
                var CompId = checkReg.CompanyId;

                var checkCompany = await _dbContext.CompanyInfo.Where(b => b.CompanyId == CompId).FirstOrDefaultAsync();
                if (checkCompany == null)
                {
                    //return null;
                    apiRes.Success = false;
                    apiRes.Message = "Invalid Registration Code.";
                    apiRes.Data = null;

                    return apiRes;
                }

                IEnumerable<BranchInfoDto> branches = await _branchRep.GetAllBranchesAsync(CompId);

                if (branches == null || branches.Count() == 0)
                {
                    apiRes.Success = false;
                    apiRes.Message = "No branches found for the company.";
                    apiRes.Data = null;
                    return apiRes;
                }


                var comRegRes = new CompanyRegistrationResponseDto
                {
                    CompanyId = checkCompany.CompanyId,
                    CompanyName = checkCompany.CompanyName
                };

                foreach (var branch in branches)
                {
                    comRegRes.Branches.Add(new BranchesDto
                    {
                        BranchName = branch.BranchName,
                        BranchId = branch.BranchId
                    });
                }

                apiRes.Data = comRegRes;
                apiRes.Success = true;
                apiRes.Message = "Link Code verified successfully.";

                return apiRes;

            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "An error occurred while verifying the Link Code.";
                apiRes.Errors = new List<string> { ex.Message };
                return apiRes;
            }
        }




    }
}
