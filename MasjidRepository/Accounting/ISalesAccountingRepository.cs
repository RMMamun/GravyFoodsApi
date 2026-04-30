using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository.Accounting
{
    public interface ISalesAccountingRepository
    {
        Task<ApiResponse<Guid>> PostSalesAsync(SalesInfoDto sale);
    }
}
