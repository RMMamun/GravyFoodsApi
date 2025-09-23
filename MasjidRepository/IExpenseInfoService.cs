using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IExpenseInfoService
    {
        Task<ExpenseInfoDto> CreateAsync(ExpenseInfoDto expenseInfo);
        Task<ExpenseInfoDto?> GetExpenseInfoById(int id, string branchId, string companyId);
        Task<IEnumerable<ExpenseInfoDto>?> GetAllExpenseInfoAsync(string branchId, string companyId);
    }
}
