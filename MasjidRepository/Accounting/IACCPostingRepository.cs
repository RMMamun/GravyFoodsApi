using GravyFoodsApi.Models.Accounting;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.MasjidRepository.Accounting
{
    public interface IACCPostingRepository
    {
        Task<ApiResponse<bool>> PostJournal(JournalInfoDto journal);
    }
}
