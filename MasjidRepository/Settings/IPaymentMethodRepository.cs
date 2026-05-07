using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.MasjidRepository.Settings
{
    public interface IPaymentMethodRepository
    {
        Task<ApiResponse<PaymentMethodsDto>> CreateAsync(PaymentMethodsDto _dto);
        Task<ApiResponse<bool>> UpdateAsync(PaymentMethodsDto _dto);
        Task<ApiResponse<bool>> DeleteAsync(Guid Id);
        Task<ApiResponse<PaymentMethodsDto>> GetByIdAsync(Guid Id);
        Task<ApiResponse<IEnumerable<PaymentMethodsDto>>> GetAllAsync();

    }
}
