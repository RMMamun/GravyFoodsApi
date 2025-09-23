using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IExpenseHeadService : IRepository<ExpenseHead>
    {
        Task<ExpenseHead> CreateAsync(ExpenseHeadDto expenseHead);
        Task<ExpenseHead?> GetExpenseHeadById(int Id, string branchId, string companyId);

        Task<IEnumerable<ExpenseHead?>> GetAllExpenseHeadAsync(string branchId, string companyId);
        
        Task<bool> UpdateExpenseHeadAsync(ExpenseHead expenseHead);
        Task<bool> DeleteExpenseHeadsAsync(int id, string branchId, string companyId);
    }
}
