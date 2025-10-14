using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using GravyFoodsApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetProductsWithDetailsAsync(string branchId, string companyId);

        Task<ProductDto> GetProductByIdAsync(string ProductId, string branchId, string companyId);
        Task<ProductDto> GetProductByBarcodeAsync(string Barcode, string branchId, string companyId );

        Task<ProductDto> UpdateProductByIdAsync(ProductDto product);

        Task<ProductDto> AddProductAsync(ProductDto product);

        Task<bool> DeleteProductAsync(string ProductId, string branchId, string companyId);
    }

    
}
