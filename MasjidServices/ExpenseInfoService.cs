using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class ExpenseInfoService : Repository<ExpenseInfo>, IExpenseInfoService
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public ExpenseInfoService(MasjidDBContext context, ITenantContextRepository tenant) : base(context)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<ApiResponse<bool>> CreateAsync(ExpenseInfoDto expenseInfo)
        {
            ApiResponse<bool> apiRes = new ();

            try
            {
                ExpenseInfo expense = new ExpenseInfo();

                expense.ExpenseHeadId = expenseInfo.ExpenseHeadId;
                expense.Description = expenseInfo.Description;
                expense.Amount = expenseInfo.Amount;
                expense.ExpenseDate = expenseInfo.ExpenseDate;
                expense.EntryDate = expenseInfo.EntryDate;
                expense.UserId = expenseInfo.UserId;
                expense.BranchId = _tenant.BranchId;
                expense.CompanyId = _tenant.CompanyId;
                


                _context.ExpenseInfo.Add(expense);
                await _context.SaveChangesAsync();

                apiRes.Data = true;
                apiRes.Success = true;
                apiRes.Message = "Expense info created successfully.";

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Failed to create expense info. Error: " + ex.Message;

                return apiRes;
            }
        }

        public async Task<ApiResponse<IEnumerable<ExpenseInfoDto>?>> GetAllExpenseInfoAsync(string branchId, string companyId)
        {
            ApiResponse<IEnumerable<ExpenseInfoDto>?> apiRes = new ();

            try
            {
                var expenseInfos = await _context.ExpenseInfo
                    .Where(e => e.BranchId == _tenant.BranchId && e.CompanyId == _tenant.CompanyId)
                    .Select(e => new ExpenseInfoDto
                    {
                        Id = e.Id,
                        ExpenseHeadId = e.ExpenseHeadId,
                        Description = e.Description,
                        Amount = e.Amount,
                        ExpenseDate = e.ExpenseDate,
                        EntryDate = e.EntryDate,
                        BranchId = e.BranchId,
                        CompanyId = e.CompanyId,
                        UserId = e.UserId,

                    
                    }).ToListAsync();

                apiRes.Data = expenseInfos;
                apiRes.Success = true;
                apiRes.Message = "Expense info retrieved successfully.";

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Failed to retrieve expense info. Error: " + ex.Message;

                return apiRes;
            }
        }


        public async Task<ApiResponse<ExpenseInfoDto?>> GetExpenseInfoById(int id, string branchId, string companyId)
        {
            ApiResponse<ExpenseInfoDto?> apiRes = new ();

            try
            {
                var expenseInfos = await _context.ExpenseInfo
                                    .Where(e => e.BranchId == _tenant.BranchId && e.CompanyId == _tenant.CompanyId)
                                    .Select(e => new ExpenseInfoDto
                                    {
                                        Id = e.Id,
                                        ExpenseHeadId = e.ExpenseHeadId,
                                        Description = e.Description,
                                        Amount = e.Amount,
                                        ExpenseDate = e.ExpenseDate,
                                        EntryDate = e.EntryDate,
                                        BranchId = e.BranchId,
                                        CompanyId = e.CompanyId,
                                        UserId = e.UserId,


                                    }).FirstOrDefaultAsync();

                apiRes.Data = expenseInfos;
                apiRes.Success = true;
                apiRes.Message = "Expense info retrieved successfully.";

                return apiRes;


            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Failed to retrieve expense info. Error: " + ex.Message;

                return apiRes;
            }
        }


        public async Task<ApiResponse<IEnumerable<ExpenseInfoDto>?>> GetExpensesInDateRangeAsync(string strSearch, DateTime fromDate, DateTime toDate)
        {
            ApiResponse<IEnumerable<ExpenseInfoDto>?> apiRes = new();

            try
            {
                if (string.IsNullOrEmpty(strSearch))
                {
                    strSearch = ""; 
                }

                var expenseInfos = await _context.ExpenseInfo
                    .Where(e => e.BranchId == _tenant.BranchId && e.CompanyId == _tenant.CompanyId
                    && (e.ExpenseDate >= fromDate && e.ExpenseDate <= toDate)
                    && ((e.Description + " " + e.Amount.ToString() + " " + e.ExpenseDate.ToString()).Contains(strSearch))
                    )

                    .Select(e => new ExpenseInfoDto
                    {
                        Id = e.Id,
                        ExpenseHeadId = e.ExpenseHeadId,
                        Description = e.Description,
                        Amount = e.Amount,
                        ExpenseDate = e.ExpenseDate,
                        EntryDate = e.EntryDate,
                        BranchId = e.BranchId,
                        CompanyId = e.CompanyId,
                        UserId = e.UserId,


                    }).ToListAsync();

                apiRes.Data = expenseInfos;
                apiRes.Success = true;
                apiRes.Message = "Expense info retrieved successfully.";

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Failed to retrieve expense info. Error: " + ex.Message;

                return apiRes;
            }
        }

    }

}
