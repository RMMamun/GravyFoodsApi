using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
                .Include(p => p.Images)
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

        public async Task<ProductDto> GetProductByIdAsync(string ProductId)
        {
            try
            {
                Product _product =  await _context.Products
                                .Include(p => p.Brand)
                                .Include(p => p.Category)
                                .Include(p => p.Images)
                                .Include(p => p.Branch)
                                .Include(p => p.Company)
                                .Include(p => p.Unit)
                                .Where(w => w.IsAvailable == true && w.IsSalable == true && w.ProductId == ProductId)
                                .FirstOrDefaultAsync();

                if (_product == null)
                {
                    return new ProductDto();
                }
                else
                {
                    ProductDto productDtos = new ProductDto
                    {
                        ProductId = _product.ProductId,
                        Name = _product.Name,
                        Description = _product.Description,
                        CategoryId = _product.CategoryId,
                        BrandId = _product.BrandId,
                        Price = _product.Price,
                        DiscountedPrice = _product.DiscountedPrice,
                        Quantity = 0,
                        IsAvailable = _product.IsAvailable,
                        IsSalable = _product.IsSalable,
                        BrandName = _product.Brand?.Name,
                        CategoryName = _product.Category?.Name,
                        ImageUrl = _product.Images.FirstOrDefault() != null ? _product.Images.FirstOrDefault().ImageUrl : null,
                        UnitId = _product.Unit != null ? _product.Unit.UnitId : null,
                        UnitName = _product.Unit != null ? _product.Unit.UnitName : null,
                        BranchId = _product.BranchId,
                        BranchName = _product.Branch != null ? _product.Branch.BranchName : null,
                        CompanyId = _product.CompanyId,
                        CompanyName = _product.Company != null ? _product.Company.CompanyName : null,

                    };

                    return productDtos;
                }
            }
            catch (Exception ex)
            {
                return new ProductDto();
            }
        }

        public async Task<ProductDto> UpdateProductByIdAsync(ProductDto product)
        {
            try
            {

                return new ProductDto();
            }
            catch (Exception ex)
            {
                return new ProductDto();
            }

        }

        public async Task<ProductDto> AddProductAsync(ProductDto product)
        {
            try
            {
                if (product != null)
                {
                    var newProduct = new Product
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Description = product.Description,
                        BrandId = product.BrandId,
                        CategoryId = product.CategoryId,
                        UnitId = product.UnitId,
                        BranchId = product.BranchId,
                        CompanyId = product.CompanyId,
                        CreatedDateTime = product.CreatedDateTime,
                        Price = product.Price,
                        DiscountedPrice = product.DiscountedPrice,
                        IsAvailable = product.IsAvailable,
                        IsSalable = product.IsSalable
                        
                    };

                    await _context.Products.AddAsync(newProduct);
                    await _context.SaveChangesAsync();
                }


                    return new ProductDto();
            }
            catch (Exception ex)
            {
                return new ProductDto();
            }
        }

    }
}
