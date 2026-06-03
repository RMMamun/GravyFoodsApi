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
                    Description = sale.Description,
                    IsPosted = true,
                    PostedAt = sale.CreatedDateTime,
                    PostedBy = "SYSTEM",
                    BranchId = _tenant.BranchId,
                    CompanyId = _tenant.CompanyId,
                    

                    JournalDetails = new List<JournalDetails>()
                };


                //sale.TotalAmount;
                //sale.TotalDiscountAmount
                //sale.TotalVATAmount
                //sale.TotalTaxAmount
                //sale.TotalPayableAmount
                //sale.TotalPaidAmount
                //sale.DueAmount



                decimal discount = (decimal)sale.TotalDiscountAmount;
                decimal NetAmount = (decimal)sale.TotalAmount - discount;
                decimal vat = (decimal)sale.TotalVATAmount;   //sale.VatAmount
                decimal tax = (decimal)sale.TotalTaxAmount;   //sale.TaxAmount
                decimal Payable = NetAmount - vat - tax;
                //decimal PaidAmount = (decimal)sale.TotalPaidAmount;   *** paid amount will be taken from the payment module. sometime customer pay partially some in cash & some in card or in mobile banking. Need to handle this
                decimal cost = 0;  //sale.TotalCost
                
                decimal dueAmount = (decimal)sale.TotalAmount - (decimal)sale.TotalPaidAmount;

                ////--DEBIT SIDE ------------------------------------------------------
                //// 💰 Debit (Cash or Receivable)
                ////*** sometime customer pay partially some in cash & some in card or in mobile banking. Need to handle this
                //journal.JournalDetails.Add(new JournalDetails
                //{
                //    AccountId = (sale.TotalAmount == sale.TotalPaidAmount)     //isCashPaid 
                //        ? settings.CashAccountId
                //        : settings.ReceivableAccountId,
                //    Debit = NetAmount,
                //    Credit = 0,
                //    BranchId = _tenant.BranchId,
                //    CompanyId = _tenant.CompanyId,
                //});

                //CASH, BANK or Mobile banking Payment
                foreach (var payment in sale.PaymentMethodsDto)
                {
                    if (payment.Amount > 0)
                    {
                        journal.JournalDetails.Add(new JournalDetails
                        {
                            AccountId = payment.AccountId,
                            Description = payment.PaymentMethodName,
                            Debit = (decimal)payment.Amount,
                            Credit = 0,
                            BranchId = _tenant.BranchId,
                            CompanyId = _tenant.CompanyId,
                        });
                    }
                }


                //Account Receivable Dr
                if (sale.TotalAmount != sale.TotalPaidAmount)
                {
                    journal.JournalDetails.Add(new JournalDetails
                    {
                        AccountId = settings.ReceivableAccountId,
                        Description = "Accounts Receivable",
                        Debit = NetAmount - (decimal)sale.TotalPaidAmount,
                        Credit = 0,
                        BranchId = _tenant.BranchId,
                        CompanyId = _tenant.CompanyId,
                    });
                }

                //Discount 
                if (discount > 0)
                {
                    journal.JournalDetails.Add(new JournalDetails
                    {
                        AccountId = settings.DiscountAccountId,
                        Description = "Sales Total Discount",
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
                    Description = "Cost of Goods Sold",
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
                    Description = "Sales Revenue",
                    Debit = 0,
                    Credit = Payable,

                    BranchId = _tenant.BranchId,
                    CompanyId = _tenant.CompanyId,
                });

                // 🧾 VAT
                if (vat > 0)
                {
                    journal.JournalDetails.Add(new JournalDetails
                    {
                        AccountId = settings.VatAccountId,
                        Description = "VAT on Sales",
                        Debit = 0,
                        Credit = vat,

                        BranchId = _tenant.BranchId,
                        CompanyId = _tenant.CompanyId,
                    });
                }

                // 🧾 Tax
                if (tax > 0)
                {
                    journal.JournalDetails.Add(new JournalDetails
                    {
                        AccountId = settings.TaxAccountId,
                        Description = "Tax on Sales",
                        Debit = 0,
                        Credit = tax,
                        BranchId = _tenant.BranchId,
                        CompanyId = _tenant.CompanyId,
                    });
                }



                // 📦 Inventory
                journal.JournalDetails.Add(new JournalDetails
                {
                    AccountId = settings.InventoryAccountId,
                    Description = "Inventory",
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
