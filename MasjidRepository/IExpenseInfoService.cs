using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IExpenseInfoService
    {
        Task<ApiResponse<bool>> CreateAsync(ExpenseInfoDto expenseInfo);
        Task<ApiResponse<ExpenseInfoDto?>> GetExpenseInfoById(int id, string branchId, string companyId);
        Task<ApiResponse<IEnumerable<ExpenseInfoDto>?>> GetAllExpenseInfoAsync(string branchId, string companyId);

        Task<ApiResponse<IEnumerable<ExpenseInfoDto>?>> GetExpensesInDateRangeAsync(string strSearch, DateTime fromDate, DateTime toDate);
    }
}
