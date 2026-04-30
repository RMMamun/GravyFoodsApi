using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.Accounting;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices.Accounting
{
    public class SalesAccountingService : ISalesAccountingRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public SalesAccountingService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }
        public async Task<ApiResponse<Guid>> PostSalesAsync(SalesInfoDto sale)
        {
            ApiResponse<Guid> apiRes = new();

            try
            {


                var settings = await _context.AccountMapping
                    .FirstAsync(x => x.CompanyId == sale.CompanyId);

                var journal = new JournalInfo
                {
                    Date = sale.CreatedDateTime,
                    ReferenceNo = sale.SalesId,
                    SourceModule = "SALES ENTRY",
                    Description = "Sales Invoice",
                    IsPosted = true,
                    PostedAt = DateTime.UtcNow,
                    PostedBy = "SYSTEM",

                    JournalDetails = new List<JournalDetails>()
                };

                decimal total = (decimal)sale.TotalAmount;
                decimal vat = 0;   //sale.VatAmount
                decimal net = total - vat;
                decimal cost = 0;  //sale.TotalCost

                // 💰 Debit (Cash or Receivable)
                //*** sometime customer pay partially some in cash & some in card or in mobile banking. Need to handle this
                journal.JournalDetails.Add(new JournalDetails
                {
                    ACCode = (sale.TotalAmount == sale.TotalPaidAmount)     //isCashPaid 
                        ? settings.CashAccountId
                        : settings.ReceivableAccountId,
                    Debit = total,
                    Credit = 0
                });

                // 💵 Sales Revenue
                journal.JournalDetails.Add(new JournalDetails
                {
                    ACCode = settings.SalesAccountId,
                    Debit = 0,
                    Credit = net
                });

                // 🧾 VAT
                if (vat > 0)
                {
                    journal.JournalDetails.Add(new JournalDetails
                    {
                        ACCode = settings.VatAccountId,
                        Debit = 0,
                        Credit = vat
                    });
                }

                // 📦 COGS
                journal.JournalDetails.Add(new JournalDetails
                {
                    ACCode = settings.CogsAccountId,
                    Debit = cost,
                    Credit = 0
                });

                // 📦 Inventory
                journal.JournalDetails.Add(new JournalDetails
                {
                    ACCode = settings.InventoryAccountId,
                    Debit = 0,
                    Credit = cost
                });

                _context.JournalInfo.Add(journal);
                await _context.SaveChangesAsync();

                apiRes.Data = journal.Id;
                apiRes.Success = true;
                apiRes.Message = "Sales posted to accounting";
                return apiRes;
            }
            catch(Exception ex)
            {
                apiRes.Success = true;
                apiRes.Message = "Sales posted to accounting";
                return apiRes;
            }
        }
    }
}
