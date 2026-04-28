using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.DTOs.Accounting;
using Microsoft.EntityFrameworkCore;

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


        public async Task<List<LedgerDto>> GetLedger(string accountId, DateTime from, DateTime to)
        {
            var query = await _context.JournalDetails
                .Where(x => x.ACCode == accountId &&
                            x.Journal.Date >= from &&
                            x.Journal.Date <= to &&
                            x.Journal.IsPosted)
                .OrderBy(x => x.Journal.Date)
                .Select(x => new
                {
                    x.Debit,
                    x.Credit,
                    x.Journal.Date,
                    x.Journal.ReferenceNo,
                    x.Journal.Description
                })
                .ToListAsync();

            decimal running = 0;

            var result = new List<LedgerDto>();

            foreach (var item in query)
            {
                running += item.Debit - item.Credit;

                result.Add(new LedgerDto
                {
                    Date = item.Date,
                    ReferenceNo = item.ReferenceNo,
                    Description = item.Description,
                    Debit = item.Debit,
                    Credit = item.Credit,
                    Balance = running
                });
            }

            return result;
        }

        public async Task<TrialBalanceDto> GetTrialBalance()
        {
            //var accounts = await _context.AccountInfo.ToListAsync();
            //var trialBalance = new TrialBalanceDto
            //{
            //    Accounts = accounts.Select(x => new AccountBalanceDto
            //    {
            //        ACCode = x.Id,
            //        AccountName = x.Name,
            //        Debit = _context.JournalDetails.Where(j => j.ACCode == x.Id).Sum(j => j.Debit),
            //        Credit = _context.JournalDetails.Where(j => j.ACCode == x.Id).Sum(j => j.Credit)
            //    }).ToList()
            //};
            //return trialBalance;

            throw new NotImplementedException();

        }
    }
}
