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

        public async Task<ExpenseInfoDto?> CreateAsync(ExpenseInfoDto expenseInfo)
        {
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
                return (expenseInfo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ExpenseInfoDto>?> GetAllExpenseInfoAsync(string branchId, string companyId)
        {
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

                return expenseInfos;
            }
            catch (Exception ex)
            {
                return (null);
            }
        }


        public async Task<ExpenseInfoDto?> GetExpenseInfoById(int id, string branchId, string companyId)
        {
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

                return expenseInfos;


            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
