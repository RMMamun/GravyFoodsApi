using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class ExpenseHeadService : Repository<ExpenseHead>, IExpenseHeadService
    {
        private readonly MasjidDBContext _context;

        public ExpenseHeadService(MasjidDBContext context) : base(context)
        {
            _context = context;
        }

        
        public Task<ExpenseHead> CreateAsync(ExpenseHeadDto expenseHead)
        {
            try
            {
                ExpenseHead expense = new ExpenseHead();
                expense.HeadName = expenseHead.HeadName;
                expense.AccountCode = expenseHead.AccountCode;
                expense.BranchId = expenseHead.BranchId;
                expense.CompanyId = expenseHead.CompanyId;
                expense.CreatedAt = expenseHead.CreatedAt;


                _context.ExpenseHead.Add(expense);
                _context.SaveChangesAsync();
                return Task.FromResult(expense);
            }
            catch (Exception ex)
            {
                return Task.FromResult<ExpenseHead>(null);
            }
        }

        public async Task<ExpenseHead?> GetExpenseHeadById(int Id,string branchId, string companyId)
        {
            try
            {
                var expenseHead = await _context.ExpenseHead.Where(e => e.Id == Id & e.BranchId == branchId & e.CompanyId == companyId).FirstOrDefaultAsync();
                return expenseHead;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<ExpenseHead>?> GetAllExpenseHeadAsync(string branchId, string companyId)
        {
            try
            {
                var expenseHeads = await _context.ExpenseHead.ToListAsync();
                return expenseHeads;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> UpdateExpenseHeadAsync(ExpenseHead expenseHead)
        {
            try
            {
                _context.ExpenseHead.Update(expenseHead);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteExpenseHeadsAsync(int id,string branchId,string companyId)
        {
            try
            {
                var expenseHead = await _context.ExpenseHead.Where(e => e.Id == id && e.BranchId == branchId & e.CompanyId == companyId).FirstOrDefaultAsync();
                if (expenseHead != null)
                {
                    _context.ExpenseHead.Remove(expenseHead);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
    }
}
