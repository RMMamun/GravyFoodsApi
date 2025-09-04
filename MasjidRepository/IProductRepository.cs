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
        Task<IEnumerable<ProductDto>> GetProductsWithDetailsAsync();

        Task<ProductDto> GetProductByIdAsync(string ProductId);

        Task<ProductDto> UpdateProductByIdAsync(ProductDto product);

        Task<ProductDto> AddProductAsync(ProductDto product);

        Task<bool> DeleteProductAsync(string ProductId);
    }

    
}
