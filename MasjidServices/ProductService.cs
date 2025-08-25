using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{

    public class ProductService : Repository<Product>, IProductRepository
    {
        private readonly MasjidDBContext _context;

        public ProductService(MasjidDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsWithDetailsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where( w => w.IsAvailable == true && w.IsSalable == true)
                .ToListAsync();
        }


        public async Task<Product?> GetProductsDetailsDto()
        {
            try
            {
                var product = await GetProductsWithDetailsAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            
        }
    }
}
