﻿using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
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

        public ProductService(MasjidDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsWithDetailsAsync(string branchId, string companyId)
        {
            try
            {


                IEnumerable<Product> product = await GetProductsDetailsAsyc(branchId,companyId);
                IEnumerable<ProductDto> productDtos = product.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    Price = p.Price,
                    DiscountedPrice = p.DiscountedPrice,
                    DiscountAmount = p.DiscountAmount,
                    Cost = p.Cost,

                    Quantity = 0,
                    IsAvailable = p.IsAvailable,
                    IsSalable = p.IsSalable,
                    BrandName = p.Brand?.Name,
                    CategoryName = p.Category?.Name,
                    ImageUrl = p.Images.FirstOrDefault() != null ? p.Images.FirstOrDefault().ImageUrl : null,
                    UnitId = p.Unit != null ? p.Unit.UnitId : null,
                    UnitType = p.Unit != null ? p.Unit.UnitName : null,
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

        public async Task<ProductDto> GetProductByBarcodeAsync(string Barcode, string branchId, string companyId)
        {
            try
            {

                Product p = await _context.Product.Where(p => p.ProductId == Barcode && p.BranchId == branchId && p.CompanyId == companyId).FirstOrDefaultAsync();
                ProductDto productDtos = new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    Price = p.Price,
                    DiscountedPrice = p.DiscountedPrice,
                    DiscountAmount = p.DiscountAmount,
                    Cost = p.Cost,

                    Quantity = 0,
                    IsAvailable = p.IsAvailable,
                    IsSalable = p.IsSalable,
                    BrandName = p.Brand?.Name != null ? p.Brand?.Name : "",
                    CategoryName = p.Category?.Name,
                    ImageUrl = p.Images.FirstOrDefault() != null ? p.Images.FirstOrDefault().ImageUrl : null,
                    UnitId = p.Unit != null ? p.Unit.UnitId : null,
                    UnitType = p.Unit != null ? p.Unit.UnitName : null,
                    BranchId = p.BranchId,
                    BranchName = p.Branch != null ? p.Branch.BranchName : null,
                    CompanyId = p.CompanyId,
                    CompanyName = p.Company != null ? p.Company.CompanyName : null,

                };

                return productDtos;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
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
                .Where(w => w.IsAvailable == true && w.IsSalable == true && w.BranchId == branchId && w.CompanyId == companyId)
                .ToListAsync();

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(string ProductId, string branchId, string companyId)
        {
            try
            {
                Product _product =  await _context.Product
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
                        DiscountAmount = _product.DiscountAmount,
                        Cost = _product.Cost,

                        Quantity = 0,
                        IsAvailable = _product.IsAvailable,
                        IsSalable = _product.IsSalable,
                        BrandName = _product.Brand?.Name,
                        CategoryName = _product.Category?.Name,
                        ImageUrl = _product.Images.FirstOrDefault() != null ? _product.Images.FirstOrDefault().ImageUrl : null,
                        UnitId = _product.Unit != null ? _product.Unit.UnitId : null,
                        UnitType = _product.Unit != null ? _product.Unit.UnitName : null,
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

        public async Task<ProductDto> UpdateProductByIdAsync(ProductDto _product)
        {
            try
            {

                var product = await _context.Product.FindAsync(_product.ProductId);
                if (product == null)
                {
                    return null;
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
                product.BranchId = _product.BranchId;
                product.CompanyId = _product.CompanyId;
                
                

                //product.Quantity = 0,
                //product.BrandName = _product.Brand?.Name,
                //product.CategoryName = _product.Category?.Name,
                //product.ImageUrl = _product.Images.FirstOrDefault() != null ? _product.Images.FirstOrDefault().ImageUrl : null,
                //product.UnitName = _product.Unit != null ? _product.Unit.UnitName : null,
                //product.BranchName = _product.Branch != null ? _product.Branch.BranchName : null,
                //product.CompanyName = _product.Company != null ? _product.Company.CompanyName : null,



                try
                {
                    await _context.SaveChangesAsync();
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
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
                        DiscountAmount = product.DiscountAmount,    
                        Cost = product.Cost,
                        IsAvailable = product.IsAvailable,
                        IsSalable = product.IsSalable,
                        
                        
                    };

                    await _context.Product.AddAsync(newProduct);
                    await _context.SaveChangesAsync();
                }


                    return new ProductDto();
            }
            catch (Exception ex)
            {
                return new ProductDto();
            }
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
