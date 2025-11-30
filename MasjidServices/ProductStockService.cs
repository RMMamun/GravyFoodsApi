using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductStockService : IProductStockRepository
    {
        private readonly MasjidDBContext _context;

        public ProductStockService(MasjidDBContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<ProductStockDto>> GetAllProductStockAsync(string branchId, string companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductStockDto> GetProductStockByIdAsync(string productId, string branchId, string companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<ProductStockDto>> UpdateProductStockAsync(ProductStockDto stockDto)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
