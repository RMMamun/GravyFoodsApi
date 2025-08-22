using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IProductVariantRepository : IRepository<ProductVariant> { }

    public class ProductVariantRepository : Repository<ProductVariant>, IProductVariantRepository
    {
        public ProductVariantRepository(MasjidDBContext context) : base(context) { }
    }
}
