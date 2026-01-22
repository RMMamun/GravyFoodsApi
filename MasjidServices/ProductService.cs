using AutoMapper;
using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Reflection.Emit;

namespace GravyFoodsApi.MasjidServices
{

    public class ProductService : Repository<Product>, IProductRepository
    {
        private readonly MasjidDBContext _context;
        private readonly IMapper _mapper;

        public ProductService(MasjidDBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsWithDetailsAsync(string branchId, string companyId)
        {
            try
            {


                //IEnumerable<Product> product = await GetProductsDetailsAsyc(branchId,companyId);
                //IEnumerable<ProductDto> productDtos = product.Select(p => new ProductDto
                //{
                //    ProductId = p.ProductId,
                //    Name = p.Name,
                //    Description = p.Description,
                //    CategoryId = p.CategoryId,
                //    BrandId = p.BrandId,
                //    Price = p.Price,
                //    DiscountedPrice = p.DiscountedPrice,
                //    DiscountAmount = p.DiscountAmount,
                //    Cost = p.Cost,

                //    Quantity = 0,
                //    IsAvailable = p.IsAvailable,
                //    IsSalable = p.IsSalable,
                //    BrandName = p.BrandDto?.Name,
                //    CategoryName = p.Category?.Name,
                //    ImageUrl = p.Images.FirstOrDefault() != null ? p.Images.FirstOrDefault().ImageUrl : null,
                //    UnitId = p.Unit != null ? p.Unit.UnitId : null,
                //    DefaultUnit = p.DefaultUnit,
                //    ProductCode = p.ProductCode,
                //    SKUCode = p.SKUCode,
                //    BranchId = p.BranchId,
                //    BranchName = p.Branch != null ? p.Branch.BranchName : null,
                //    CompanyId = p.CompanyId,
                //    CompanyName = p.Company != null ? p.Company.CompanyName : null,

                //});

                var products = await GetProductsDetailsAsyc(branchId, companyId);
                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

                return productDtos;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }

        }

        public async Task<ApiResponse<ProductDto>> GetProductByBarcodeAsync(string ProductId,string Barcode, string branchId, string companyId)
        {
            ApiResponse<ProductDto> apiRes = new ApiResponse<ProductDto>();

            try
            {

                Product? products = await _context.Product.Where(p => (p.ProductCode == ProductId || p.SKUCode == Barcode) && p.BranchId == branchId && p.CompanyId == companyId).FirstOrDefaultAsync();
                
                var productDtos = _mapper.Map<ProductDto>(products);

                apiRes.Data = productDtos;
                apiRes.Success = true;
                apiRes.Message = "Product retrieved successfully.";
                return apiRes;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                apiRes.Success = false;
                apiRes.Message = "Error retrieving product.";
                apiRes.Errors = new List<string> { ex.Message };

                return apiRes;
            }
        }

        public async Task<IEnumerable<Product?>> GetProductsDetailsAsyc(string branchId, string companyId)
        {
            try
            {

                return await _context.Product
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Branch)
                .Include(p => p.Company)
                .Include(p => p.Unit)
                .Where(w =>  w.BranchId == branchId && w.CompanyId == companyId)
                .ToListAsync();

                //w.IsAvailable == true && w.IsSalable == true &&

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }
        }

        public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(string ProductId, string branchId, string companyId)
        {
            ApiResponse<ProductDto> apiRes = new ApiResponse<ProductDto>();

            try
            {
                Product? _product =  await _context.Product
                                .Include(p => p.Brand)
                                .Include(p => p.Category)
                                .Include(p => p.Images)
                                .Include(p => p.Branch)
                                .Include(p => p.Company)
                                .Include(p => p.Unit)
                                .Where(w => w.BranchId == branchId && w.CompanyId == companyId && w.ProductId == ProductId)
                                .FirstOrDefaultAsync();

                if (_product == null)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Product not found!";
                    return apiRes;
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
                        DiscountAmount = _product.DiscountAmount,
                        Cost = _product.Cost,

                        Quantity = 0,
                        IsAvailable = _product.IsAvailable,
                        IsSalable = _product.IsSalable,
                        BrandName = _product.Brand?.Name,
                        CategoryName = _product.Category?.Name,
                        ImageUrl = _product.Images.FirstOrDefault() != null ? _product.Images.FirstOrDefault().ImageUrl : null,
                        UnitId = _product.Unit != null ? _product.Unit.UnitId : null,
                        DefaultUnit = _product.DefaultUnit,
                        ProductCode = _product.ProductCode,
                        SKUCode = _product.SKUCode,
                        BranchId = _product.BranchId,
                        BranchName = _product.Branch != null ? _product.Branch.BranchName : null,
                        CompanyId = _product.CompanyId,
                        CompanyName = _product.Company != null ? _product.Company.CompanyName : null,
                        CreatedDateTime = _product.CreatedDateTime,
                        ExpiryDate = _product.ExpiryDate,
                        IsSerialBased = _product.IsSerialBased,
                        StockLimit = _product.StockLimit
                        
                        

                    };

                    apiRes.Data = productDtos;
                    apiRes.Success = true;
                    apiRes.Message = "Product retrieved successfully.";

                    return apiRes;
                }
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error retrieving product.";
                apiRes.Errors = new List<string> { ex.Message };
                return apiRes;
            }
        }

        public async Task<bool> UpdateProductByIdAsync(ProductDto _product)
        {
            try
            {

                var product = await _context.Product.FindAsync(_product.ProductId);
                if (product == null)
                {
                    return false;
                }

                //product.ProductId = _product.ProductId;
                product.Name = _product.Name;
                product.Description = _product.Description;
                product.CategoryId = _product.CategoryId;
                product.BrandId = _product.BrandId;
                product.Price = _product.Price;
                product.DiscountedPrice = _product.DiscountedPrice;
                product.DiscountAmount = _product.DiscountAmount;
                product.Cost = _product.Cost;
                product.IsAvailable = _product.IsAvailable;
                product.IsSalable = _product.IsSalable;
                product.UnitId = _product.UnitId;
                product.DefaultUnit = _product.DefaultUnit;
                product.ProductCode = _product.ProductCode;
                product.SKUCode = _product.SKUCode;
                product.BranchId = _product.BranchId;
                product.CompanyId = _product.CompanyId;
                product.ExpiryDate = _product.ExpiryDate;
                product.IsSerialBased = _product.IsSerialBased;
                product.StockLimit = _product.StockLimit;
                //product.CreatedDateTime = _product.CreatedDateTime;  //Not updatable



                //product.Quantity = 0,
                //product.BrandName = _product.BrandDto?.Name,
                //product.CategoryName = _product.Category?.Name,
                //product.ImageUrl = _product.Images.FirstOrDefault() != null ? _product.Images.FirstOrDefault().ImageUrl : null,
                //product.UnitName = _product.Unit != null ? _product.Unit.UnitName : null,
                //product.BranchName = _product.Branch != null ? _product.Branch.BranchName : null,
                //product.CompanyName = _product.Company != null ? _product.Company.CompanyName : null,



                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<ApiResponse<ProductDto>> AddProductAsync(ProductDto product)
        {
            try
            {

                    var newProduct = new Product
                    {
                        ProductId = GenerateProductId(product.CompanyId),

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
                        DiscountAmount = product.DiscountAmount,
                        Cost = product.Cost,
                        IsAvailable = product.IsAvailable,
                        IsSalable = product.IsSalable,
                        DefaultUnit = product.DefaultUnit,
                        ProductCode = product.ProductCode,
                        SKUCode = product.SKUCode,
                        StockLimit = product.StockLimit,
                        ExpiryDate = product.ExpiryDate,
                        IsSerialBased = product.IsSerialBased,
                        
                        


                    };

                    await _context.Product.AddAsync(newProduct);
                    await _context.SaveChangesAsync();

                    product.ProductId = newProduct.ProductId;

                    //return product;
                    return new ApiResponse<ProductDto>
                    {
                        Success = true,
                        Message = "Product saved successfully.",
                        Data = product,
                        Errors = null
                    };

            }
            catch (Exception ex)
            {
                //return new ProductDto();
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Product could not be saved!!",
                    Data = null,
                    Errors = new List<string> { ex.Message }

                };

            }
        }

        private string GenerateProductId(string companyCode)
        {
            string productId = companyCode + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper();
            return productId;
        }


        public async Task<bool> DeleteProductAsync(string ProductId, string branchId, string companyId)
        {
            try
            {
                var product = await _context.Product.FindAsync(ProductId);
                if (product == null)
                {
                    return false;
                }

                //ProductImageService _productImageService = new ProductImageService(_context);
                //await _productImageService.DeleteProductImages(ProductId);


                await _context.ProductImages
                .Where(od => od.ProductId == ProductId)
                .ExecuteDeleteAsync();

                await _context.Product
                .Where(od => od.ProductId == ProductId)
                .ExecuteDeleteAsync();

                //_context.Products.Remove(product);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
