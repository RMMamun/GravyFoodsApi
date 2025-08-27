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

        public async Task<IEnumerable<ProductDto>> GetProductsWithDetailsAsync()
        {
            try
            {

                IEnumerable<Product> product = await GetProductsDetailsAsyc();
                IEnumerable<ProductDto> productDtos = product.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    Price = p.Price,
                    DiscountedPrice = p.DiscountedPrice,
                    Quantity = 0,
                    IsAvailable = p.IsAvailable,
                    IsSalable = p.IsSalable,
                    BrandName = p.Brand?.Name,
                    CategoryName = p.Category?.Name,
                    ImageUrl = p.Images.FirstOrDefault() != null ? p.Images.FirstOrDefault().ImageUrl : null,
                    UnitId = p.Unit != null ? p.Unit.UnitId : null,
                    UnitName = p.Unit != null ? p.Unit.UnitName : null,
                    BranchId = p.BranchId,
                    BranchName = p.Branch != null ? p.Branch.BranchName : null,
                    CompanyId = p.CompanyId,
                    CompanyName = p.Company != null ? p.Company.CompanyName : null,

                });

                return productDtos;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }

        }


        public async Task<IEnumerable<Product?>> GetProductsDetailsAsyc()
        {
            try
            {

                return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images )
                .Include(p => p.Branch)
                .Include(p => p.Company)
                .Include(p => p.Unit)
                .Where(w => w.IsAvailable == true && w.IsSalable == true)
                .ToListAsync();

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }

            
        }
    }
}
