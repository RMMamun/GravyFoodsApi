using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IExpenseHeadService : IRepository<ExpenseHead>
    {
        Task<ApiResponse<bool>> CreateAsync(ExpenseHeadDto expenseHead);
        
        Task<ApiResponse<ExpenseHeadDto?>> GetExpenseHeadById(int Id);

        Task<ApiResponse<IEnumerable<ExpenseHeadDto?>>> GetAllExpenseHeadAsync();
        
        Task<ApiResponse<bool>> UpdateExpenseHeadAsync(ExpenseHeadDto expenseHead);
        Task<ApiResponse<bool>> DeleteExpenseHeadsAsync(int id);
    }
}
