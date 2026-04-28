using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.MasjidRepository.Accounting
{
    public interface IAccountingReportRepository
    {
        Task<List<LedgerDto>> GetLedger(string accountId, DateTime from, DateTime to);
        Task<TrialBalanceDto> GetTrialBalance();
    }
}
