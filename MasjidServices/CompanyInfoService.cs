using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class CompanyInfoService : ICompanyInfoService
    {
        private readonly MasjidDBContext _context;
        private readonly IBranchInfoRepository _branchRep;

        public CompanyInfoService(MasjidDBContext context, IBranchInfoRepository branchRep)
        {
            _context = context;
            _branchRep = branchRep;
        }

        public async Task<bool> CreateCompanyInfoAsync(CompanyInfoDto companyInfo)
        {

            //First create Company
            //2nd Create result with CompanyId
            //All default settings like user, roles, permissions, menus, customer, supplier, products etc.,


            throw new NotImplementedException();
        }

        public async Task<CompanyInfoDto> GetCompanyInfoAsync(string companyId)
        {
            var company = await _context.CompanyInfo.FindAsync(companyId);
            if (company == null)
            {
                return null;
            }
            return new CompanyInfoDto
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                CompanyCode = company.CompanyCode,
                Address = company.Address,
                Phone = company.Phone,
                Mobile = company.Mobile,
                Email = company.Email,
                Website = company.Website
            };
        }
        public async Task<bool> UpdateCompanyInfoAsync(CompanyInfoDto companyInfo)
        {
            throw new NotImplementedException();
        }


        public async Task<ApiResponse<CompanyRegistrationResponseDto?>> GetCompanyRegistrationVerificationAsync(Guid RegCode)
        {

            ApiResponse<CompanyRegistrationResponseDto?> apiRes = new ApiResponse<CompanyRegistrationResponseDto?>();
            try
            {


                var result = await _context.CompanyInfo.Where(b => b.RegCode == RegCode).FirstOrDefaultAsync();
                if (result == null)
                {
                    //return null;
                    apiRes.Success = false;
                    apiRes.Message = "Invalid Registration Code.";
                    apiRes.Data = null;

                    return apiRes;
                }

                IEnumerable<BranchInfoDto> branches = await _branchRep.GetAllBranchesAsync(result.CompanyId);

                if (branches == null || branches.Count() == 0)
                {
                    apiRes.Success = false;
                    apiRes.Message = "No branches found for the company.";
                    apiRes.Data = null;
                    return apiRes;
                }


                var comRegRes = new CompanyRegistrationResponseDto
                {
                    CompanyName = result.CompanyName
                };

                foreach (var branch in branches)
                {
                    comRegRes.Branches.Add(new BranchesDto
                    {
                        BranchName = branch.BranchName,
                        LinkCode = branch.LinkCode
                    });
                }


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
