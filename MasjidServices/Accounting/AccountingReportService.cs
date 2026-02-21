using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.MasjidServices.Accounting
{
    public class AccountingReportService : IAccountingReportRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;
        public AccountingReportService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }


        public async Task<List<LedgerDto>> GetLedger(Guid accountId)
        {
            //return await _context.JournalDetails
            //    .Where(x => x.AccountId == accountId)
            //    .Select(x => new LedgerDto
            //    {
            //        Date = x.JournalInfo.Date,
            //        Debit = x.Debit,
            //        Credit = x.Credit
            //    }).ToListAsync();

            throw new NotImplementedException();
        }

        public async Task<TrialBalanceDto> GetTrialBalance()
        {
            //var accounts = await _context.AccountInfo.ToListAsync();
            //var trialBalance = new TrialBalanceDto
            //{
            //    Accounts = accounts.Select(x => new AccountBalanceDto
            //    {
            //        AccountId = x.Id,
            //        AccountName = x.Name,
            //        Debit = _context.JournalDetails.Where(j => j.AccountId == x.Id).Sum(j => j.Debit),
            //        Credit = _context.JournalDetails.Where(j => j.AccountId == x.Id).Sum(j => j.Credit)
            //    }).ToList()
            //};
            //return trialBalance;

            throw new NotImplementedException();

        }
    }
}
