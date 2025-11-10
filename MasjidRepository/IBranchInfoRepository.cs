using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IBranchInfoRepository
    {
        Task<BranchInfoDto> GetBranchInfoAsync(string BranchId, string companyId);
        Task<IEnumerable<BranchInfoDto>> GetAllBranchesAsync(string companyId);

        Task<bool> UpdateBranchInfoAsync(BranchInfoDto BranchInfo);
        Task<bool> CreateBranchInfoAsync(BranchInfoDto BranchInfo);
    }
}
