using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using System;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IProductStockRepository : IRepository<ProductStock> { }

    public class ProductStockRepository : Repository<ProductStock>, IProductStockRepository
    {
        public ProductStockRepository(MasjidDBContext context) : base(context) { }
    }
}
