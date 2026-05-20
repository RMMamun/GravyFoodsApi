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
                    .FirstAsync(x => x.CompanyId == _tenant.CompanyId);

                if (settings == null || string.IsNullOrEmpty(settings.CompanyId) == true )
                {
                    apiRes.Success = false;
                    apiRes.Message = "Account mapping not found!";
                    return apiRes;
                }

                var journal = new JournalInfo
                {
                    Date = sale.CreatedDateTime,
                    ReferenceNo = sale.SalesId,
                    SourceModule = "SALES ENTRY",
                    Description = "Sales Invoice",
                    IsPosted = true,
                    PostedAt = sale.CreatedDateTime,
                    PostedBy = "SYSTEM",
                    BranchId = _tenant.BranchId,
                    CompanyId = _tenant.CompanyId,
                    

                    JournalDetails = new List<JournalDetails>()
                };

                decimal discount = 0;
                decimal total = (decimal)sale.TotalAmount - discount;
                decimal vat = 0;   //sale.VatAmount
                decimal net = total - vat;
                decimal cost = 0;  //sale.TotalCost
                
                decimal dueAmount = (decimal)sale.TotalAmount - (decimal)sale.TotalPaidAmount;

                //--DEBIT SIDE ------------------------------------------------------
                // 💰 Debit (Cash or Receivable)
                //*** sometime customer pay partially some in cash & some in card or in mobile banking. Need to handle this
                journal.JournalDetails.Add(new JournalDetails
                {
                    AccountId = (sale.TotalAmount == sale.TotalPaidAmount)     //isCashPaid 
                        ? settings.CashAccountId
                        : settings.ReceivableAccountId,
                    Debit = total,
                    Credit = 0,
                    BranchId = _tenant.BranchId,
                    CompanyId = _tenant.CompanyId,
                });

                //Account Receivable Dr
                if (sale.TotalAmount != sale.TotalPaidAmount)
                {
                    journal.JournalDetails.Add(new JournalDetails
                    {
                        AccountId = settings.ReceivableAccountId,
                        Debit = total - (decimal)sale.TotalPaidAmount,
                        Credit = 0,
                        BranchId = _tenant.BranchId,
                        CompanyId = _tenant.CompanyId,
                    });
                }

                //Dicount 
                if (discount > 0)
                {
                    journal.JournalDetails.Add(new JournalDetails
                    {
                        AccountId = settings.DiscountAccountId,
                        Debit = discount,
                        Credit = 0,
                        BranchId = _tenant.BranchId,
                        CompanyId = _tenant.CompanyId,
                    });
                }

                // 📦 COGS
                journal.JournalDetails.Add(new JournalDetails
                {
                    AccountId = settings.CogsAccountId,
                    Debit = cost,
                    Credit = 0,

                    BranchId = _tenant.BranchId,
                    CompanyId = _tenant.CompanyId,
                });
                //----------------------------------------------------

                // CREDIT SIDE
                // 💵 Sales Revenue
                journal.JournalDetails.Add(new JournalDetails
                {
                    AccountId = settings.SalesAccountId,
                    Debit = 0,
                    Credit = net,

                    BranchId = _tenant.BranchId,
                    CompanyId = _tenant.CompanyId,
                });

                // 🧾 VAT
                if (vat > 0)
                {
                    journal.JournalDetails.Add(new JournalDetails
                    {
                        AccountId = settings.VatAccountId,
                        Debit = 0,
                        Credit = vat,

                        BranchId = _tenant.BranchId,
                        CompanyId = _tenant.CompanyId,
                    });
                }

                

                // 📦 Inventory
                journal.JournalDetails.Add(new JournalDetails
                {
                    AccountId = settings.InventoryAccountId,
                    Debit = 0,
                    Credit = cost,

                    BranchId = _tenant.BranchId,
                    CompanyId = _tenant.CompanyId,
                    
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
                apiRes.Success = false;
                apiRes.Message = "Sales posted to accounting";
                return apiRes;
            }
        }
    }
}
