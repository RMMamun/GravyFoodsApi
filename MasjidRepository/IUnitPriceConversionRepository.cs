using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IUnitPriceConversionRepository
    {
        Task<ApiResponse<List<ProductUnitsWithPriceDto>>> GetProductUnitsWithPriceAsync(string productId);
    }
}
