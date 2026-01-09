using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.ComponentModel.Design;

namespace GravyFoodsApi.MasjidServices
{
    public class BranchInfoService : IBranchInfoRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ICompanyInfoService _companyService;
        public BranchInfoService(MasjidDBContext context, ICompanyInfoService companyService)
        {
            _context = context;
            _companyService = companyService;
        }

        public async Task<bool> CreateBranchInfoAsync(BranchInfoDto BranchInfo)
        {

            var branch = new BranchInfo
            {
                BranchId = GenerateBranchId(BranchInfo.CompanyId),
                BranchName = BranchInfo.BranchName,
                Address = BranchInfo.Address,
                Phone = BranchInfo.Phone,
                Mobile = BranchInfo.Mobile,
                Email = BranchInfo.Email,
                Website = BranchInfo.Website,
                CompanyId = BranchInfo.CompanyId,
                LinkCode = BranchInfo.LinkCode
            };
            _context.BranchInfo.Add(branch);
            await _context.SaveChangesAsync();
            return true;

        }

        private string GenerateBranchId(string companyCode)
        {
            // Generate a unique BranchId, e.g., using GUID
            return companyCode + Guid.NewGuid().ToString();
        }

        public async Task<BranchInfoDto> GetBranchInfoAsync(string BranchId, string companyId)
        {
            var Branch = await _context.BranchInfo.Where(b => b.CompanyId == companyId && b.BranchId == BranchId).FirstOrDefaultAsync();
            if (Branch == null)
            {
                return null;
            }
            return new BranchInfoDto
            {
                BranchId = Branch.BranchId,
                BranchName = Branch.BranchName,
                Address = Branch.Address,
                Phone = Branch.Phone,
                Mobile = Branch.Mobile,
                Email = Branch.Email,
                Website = Branch.Website,
                CompanyId = Branch.CompanyId,
                LinkCode = Branch.LinkCode


            };
        }




        public async Task<IEnumerable<BranchInfoDto>> GetAllBranchesAsync(string companyId)
        {

            IEnumerable<BranchInfo> branches = await _context.BranchInfo.Where(b => b.CompanyId == companyId).ToListAsync();
            IEnumerable<BranchInfoDto> branchesDto = branches.Select(p => new BranchInfoDto
            {
                CompanyId = p.CompanyId,
                BranchId = p.BranchId,
                BranchName = p.BranchName,
                Address = p.Address,
                Phone = p.Phone,
                Mobile = p.Mobile,
                Email = p.Email,
                Website = p.Website,
                LinkCode = p.LinkCode


            });

            return branchesDto;

        }

        public async Task<bool> UpdateBranchInfoAsync(BranchInfoDto branchDo)
        {
            var Branch = await _context.BranchInfo.Where(b => b.CompanyId == branchDo.CompanyId && b.BranchId == branchDo.BranchId).FirstOrDefaultAsync();
            if (Branch == null)
            {
                return false;
            }

            Branch.BranchId = branchDo.BranchId;
            Branch.BranchName = branchDo.BranchName;
            Branch.Address = branchDo.Address;
            Branch.Phone = branchDo.Phone;
            Branch.Mobile = branchDo.Mobile;
            Branch.Email = branchDo.Email;
            Branch.Website = branchDo.Website;
            Branch.CompanyId = branchDo.CompanyId;
            Branch.LinkCode = branchDo.LinkCode;


            _context.BranchInfo.Update(Branch);
            await _context.SaveChangesAsync();
            return true;

        }


        public class CompanyBranchDto
        {
            public string CompanyCode { get; set; }
            public string CompanyName { get; set; }
            public string BranchCode { get; set; }
            public string BranchName { get; set; }
            public string Address { get; set; }
        }

        public async Task<ApiResponse<CompanyBranchDto>> GetLinkCodeVerificationAsync(string LinkCode)
        {

            ApiResponse<CompanyBranchDto> apiRes = new ApiResponse<CompanyBranchDto>();

            //Split the code in 2 parts CompanyCode and BranchCode
            string companyCode = LinkCode.Substring(0, 20);
            string branchCode = LinkCode.Substring(20, 20);

            var Branch = await _context.BranchInfo.Where(b => b.LinkCode == LinkCode && b.BranchId == branchCode).FirstOrDefaultAsync();
            if (Branch == null)
            {
                //return null;
                apiRes.Success = false;
                apiRes.Message = "Invalid Link Code.";

                return apiRes;
            }


            var Company = await _companyService.GetCompanyInfoAsync(companyCode);
            if (Company == null)
            {
                apiRes.Success = false;
                apiRes.Message = "Invalid Link Code.";

                return apiRes;
            }

            apiRes.Data = new CompanyBranchDto
            {
                CompanyCode = Company.CompanyId,
                CompanyName = Company.CompanyName,
                BranchCode = Branch.BranchId,
                BranchName = Branch.BranchName,
                Address = Branch.Address
            };

            apiRes.Success = true;
            apiRes.Message = "Link Code verified successfully.";

            return apiRes;
        }







    }
}
