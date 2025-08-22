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
        Task<IEnumerable<Product>> GetProductsWithDetailsAsync();
    }

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly MasjidDBContext _context;

        public ProductRepository(MasjidDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsWithDetailsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}
