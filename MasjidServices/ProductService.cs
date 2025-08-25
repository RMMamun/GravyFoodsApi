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


        public async Task<IEnumerable<ProductDto?>> GetProductsDetailsDto()
        {
            try
            {
                IEnumerable<Product> product = await GetProductsWithDetailsAsync();
                IEnumerable<ProductDto> productDtos = product.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    Price = p.Price,
                    DiscountedPrice = p.DiscountedPrice,
                    IsAvailable = p.IsAvailable,
                    IsSalable = p.IsSalable,
                    BrandName = p.Brand != null ? p.Brand.Name : null,
                    CategoryName = p.Category != null ? p.Category.Name : null,
                    ImageUrl = p.Images.FirstOrDefault() != null ? p.Images.FirstOrDefault().ImageUrl : null,
                    UnitId = p.Unit != null ? p.Unit.UnitId : null,
                    UnitName = p.Unit != null ? p.Unit.UnitName : null

                });

                return productDtos;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }

            
        }
    }
}
