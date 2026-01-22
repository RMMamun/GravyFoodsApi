using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class CompanyInfoService : ICompanyInfoService
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public CompanyInfoService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
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
            var company = await _context.CompanyInfo.FindAsync(_tenant.CompanyId);
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



    }
}
