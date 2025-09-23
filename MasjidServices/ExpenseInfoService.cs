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

        public ExpenseInfoService(MasjidDBContext context) : base(context)
        {
            _context = context;
        }

        public Task<ExpenseInfoDto> CreateAsync(ExpenseInfoDto expenseInfo)
        {
            try
            {
                ExpenseInfo expense = new ExpenseInfo();

                expense.ExpenseHeadId = expenseInfo.ExpenseHeadId;
                expense.Description = expenseInfo.Description;
                expense.Amount = expenseInfo.Amount;
                expense.ExpenseDate = expenseInfo.ExpenseDate;
                expense.EntryDate = expenseInfo.EntryDate;
                expense.BranchId = expenseInfo.BranchId;
                expense.CompanyId = expenseInfo.CompanyId;
                expense.UserId = expenseInfo.UserId;


                _context.ExpenseInfo.Add(expense);
                _context.SaveChangesAsync();
                return Task.FromResult(expenseInfo);
            }
            catch (Exception ex)
            {
                return Task.FromResult<ExpenseInfoDto>(null);
            }
        }

        public Task<IEnumerable<ExpenseInfoDto>?> GetAllExpenseInfoAsync(string branchId, string companyId)
        {
            try
            {
                var expenseInfos = _context.ExpenseInfo
                    .Where(e => e.BranchId == branchId && e.CompanyId == companyId)
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

                    
                    }).AsAsyncEnumerable();

            return Task.FromResult<IEnumerable<ExpenseInfoDto>?>((IEnumerable<ExpenseInfoDto>?)expenseInfos);
        }
            catch (Exception ex)
            {
                return Task.FromResult<IEnumerable<ExpenseInfoDto>?>(null);
            }
    }


        public Task<ExpenseInfoDto?> GetExpenseInfoById(int id, string branchId, string companyId)
        {
            try
            {
                var expenseInfos = _context.ExpenseInfo
                                    .Where(e => e.BranchId == branchId && e.CompanyId == companyId)
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

                return Task.FromResult<ExpenseInfoDto?>(expenseInfos.Result);


            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
