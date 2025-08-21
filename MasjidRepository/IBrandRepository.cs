using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IBrandRepository : IRepository<Brand> { }

    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(MasjidDBContext context) : base(context) { }
    }
}
