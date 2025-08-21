using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using GravyFoodsApi.Data;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory> { }

    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(MasjidDBContext context) : base(context) { }
    }
}
