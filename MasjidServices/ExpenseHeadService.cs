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
        private readonly ITenantContextRepository _tenant;

        public ExpenseHeadService(MasjidDBContext context, ITenantContextRepository tenant) : base(context)
        {
            _context = context;
            _tenant = tenant;
        }

        
        public async Task<ApiResponse<bool>> CreateAsync(ExpenseHeadDto expenseHead)
        {
            ApiResponse<bool> apiRes = new();
            try
            {
                ExpenseHead expense = new ExpenseHead();

                expense.HeadName = expenseHead.HeadName;
                expense.AccountCode = expenseHead.AccountCode;
                expense.BranchId = _tenant.BranchId;
                expense.CompanyId = _tenant.CompanyId;
                expense.CreatedAt = expenseHead.CreatedAt;


                _context.ExpenseHead.Add(expense);
                await _context.SaveChangesAsync();

                apiRes.Success = true;
                apiRes.Message = "Created successfully.";
                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;
                return apiRes;
            }
        }

        public async Task<ApiResponse<ExpenseHeadDto?>> GetExpenseHeadById(int Id)
        {
            ApiResponse<ExpenseHeadDto> apiRes = new();

            try
            {
                var result = await _context.ExpenseHead.Where(e => e.Id == Id & e.BranchId == _tenant.BranchId & e.CompanyId == _tenant.CompanyId).FirstOrDefaultAsync();
                if (result == null)
                {
                    apiRes.Success = true;
                    apiRes.Message = "Expense head not found for the ID: " + Id.ToString();
                    apiRes.Data = null;
                    return apiRes;
                }

                var expenseHead = new ExpenseHeadDto
                {
                    Id = result.Id,
                    HeadName = result.HeadName,
                    AccountCode = result.AccountCode,
                    CreatedAt = result.CreatedAt,
                };


                apiRes.Data = expenseHead;
                apiRes.Success = true;
                apiRes.Message = "Expense head found for the ID: " + Id.ToString();
                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Message = ex.Message;
                apiRes.Success = false;
                return apiRes;
            }
        }

        public async Task<ApiResponse<IEnumerable<ExpenseHeadDto>?>> GetAllExpenseHeadAsync()
        {
            ApiResponse<IEnumerable<ExpenseHeadDto>?> apiRes = new();

            try
            {
                var result = await _context.ExpenseHead.Where( w => w.BranchId == _tenant.BranchId && w.CompanyId == _tenant.CompanyId) .ToListAsync();
                if (result.Count() == 0)
                {
                    apiRes.Success = true;
                    apiRes.Data = null;
                    apiRes.Message = "No expense head found";

                    return apiRes;
                }
                IEnumerable<ExpenseHeadDto> expenseHeads = result.Select(p => new ExpenseHeadDto
                {
                    Id = p.Id,
                    HeadName = p.HeadName,
                    AccountCode = p.AccountCode,
                    CreatedAt = p.CreatedAt
                });

                apiRes.Success = true;
                apiRes.Message = "Expense heads found";
                apiRes.Data = expenseHeads;
                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Message = ex.Message;
                apiRes.Success = false;
                apiRes.Data = null;
                return apiRes;
            }
        }
        public async Task<ApiResponse<bool>> UpdateExpenseHeadAsync(ExpenseHeadDto dto)
        {
            ApiResponse<bool> apiRes = new();
            try
            {
                //check existence 
                var result = _context.ExpenseHead.FirstOrDefault(w => w.Id == dto.Id && w.BranchId == _tenant.BranchId && w.CompanyId == _tenant.CompanyId);
                if (result == null)
                {
                    apiRes.Success = false;
                    apiRes.Message = "";
                    apiRes.Data = false;
                    return apiRes;
                }

                
                result.HeadName = dto.HeadName;
                result.AccountCode = dto.AccountCode;
                result.CreatedAt = dto.CreatedAt;


                _context.ExpenseHead.Update(result);
                await _context.SaveChangesAsync();

                apiRes.Success = true;
                apiRes.Message = "Updated successfully.";
                apiRes.Data = true;

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Message = ex.Message;
                apiRes.Success = false;
                apiRes.Data = false;
                return apiRes;
            }
        }

        public async Task<ApiResponse<bool>> DeleteExpenseHeadsAsync(int id)
        {
            ApiResponse<bool> apiRes = new();

            try
            {
                var expenseHead = await _context.ExpenseHead.Where(e => e.Id == id && e.BranchId == _tenant.BranchId & e.CompanyId == _tenant.CompanyId).FirstOrDefaultAsync();
                if (expenseHead == null)
                {
                    apiRes.Message = "No record found to delete.";
                    apiRes.Success = true;
                    apiRes.Data = false;
                    return apiRes;
                }

                _context.ExpenseHead.Remove(expenseHead);
                await _context.SaveChangesAsync();

                apiRes.Message = "Record deleted successfully.";
                apiRes.Success = true;
                apiRes.Data = true;
                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Message = ex.Message;
                apiRes.Success = false;
                apiRes.Data = false;
                return apiRes;
            }
        }

    }
}
