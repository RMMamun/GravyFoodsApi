using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.MasjidRepository.Accounting
{
    public interface IJournalRepository
    {
        Task<ApiResponse<Guid>> CreateAsync(JournalInfoDto dto);
        Task<ApiResponse<bool>> PostAsync(Guid journalId, string user);
        Task<ApiResponse<Guid>> ReverseAsync(Guid journalId, string user);

    }
}
