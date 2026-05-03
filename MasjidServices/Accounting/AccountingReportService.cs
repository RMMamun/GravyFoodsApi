using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.DTOs.Accounting;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            try
            {

                decimal opening = await LedgerOpeningBalanceAsync(accountId, from);

                var query = await _context.JournalDetails
                    .Where(x => x.AccountId.ToString() == accountId &&
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
                        x.Journal.Description,
                        x.Journal.SourceModule,
                        x.Journal.IsPosted,

                    })
                    .ToListAsync();

                //decimal running = 0;

                //var result = new List<LedgerDto>();

                //foreach (var item in query)
                //{
                //    running += item.Debit - item.Credit;

                //    result.Add(new LedgerDto
                //    {
                //        Date = item.Date,
                //        ReferenceNo = item.ReferenceNo,
                //        Description = item.Description,
                //        Debit = item.Debit,
                //        Credit = item.Credit,
                //        Balance = running
                //    });
                //}

                decimal running = opening;

                var result = new List<LedgerDto>();

                // Opening row
                result.Add(new LedgerDto
                {
                    Date = from,
                    Description = "Opening Balance",
                    Balance = running
                });

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
                        Balance = running,
                        SourceModule = item.SourceModule,
                        IsPosted = item.IsPosted
                    });
                }

                return result;

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error fetching ledger: {ex.Message}");
                return new List<LedgerDto>(); // Return an empty list or handle it as per your requirements
            }
        }


        private async Task<decimal> LedgerOpeningBalanceAsync(string accountId, DateTime fromDate)
        {
            try
            {
                var opening = await _context.JournalDetails
                    .Where(x => x.AccountId.ToString() == accountId &&
                    x.Journal.Date < fromDate &&
                    x.Journal.IsPosted)
                    .SumAsync(x => x.Debit - x.Credit);

                return opening;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error calculating opening balance: {ex.Message}");
                return 0; // Return 0 or handle it as per your requirements
            }
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
