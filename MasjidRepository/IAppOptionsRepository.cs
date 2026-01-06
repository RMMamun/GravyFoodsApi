using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IAppOptionsRepository
    {
        Task<ApiResponse<IEnumerable<AppOptionDto>>> GetAppOptionsAsync(string branchId, string companyId);
    }
}
