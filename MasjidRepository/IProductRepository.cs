using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetProductsWithDetailsAsync();

        Task<ApiResponse<ProductDto>> GetProductByIdAsync(string ProductId);
        Task<ApiResponse<ProductDto>> GetProductByBarcodeAsync(string ProductId, string Barcode);

        Task<ApiResponse<ProductDto>> GetProductCostByIdAsync(string ProductId);

        Task<bool> UpdateProductByIdAsync(ProductDto product);

        Task<ApiResponse<ProductDto>> AddProductAsync(ProductDto product);

        Task<bool> DeleteProductAsync(string ProductId);
    }

    
}
