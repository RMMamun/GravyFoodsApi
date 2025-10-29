using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ICompanyInfoService
    {

        Task<CompanyInfoDto> GetCompanyInfoAsync(string companyId);

        Task<bool> UpdateCompanyInfoAsync(CompanyInfoDto companyInfo);
        Task<bool> CreateCompanyInfoAsync(CompanyInfoDto companyInfo);

    }
}
