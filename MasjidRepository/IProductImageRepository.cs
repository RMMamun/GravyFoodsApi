using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using System;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IProductImageRepository : IRepository<ProductImage> { }

    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(MasjidDBContext context) : base(context) { }
    }
}
