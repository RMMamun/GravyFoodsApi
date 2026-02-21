using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.MasjidRepository.Accounting
{
    public interface IAccountingReportRepository
    {
        Task<List<LedgerDto>> GetLedger(Guid accountId);
        Task<TrialBalanceDto> GetTrialBalance();
    }
}
