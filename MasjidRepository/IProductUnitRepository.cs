using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using GravyFoodsApi.Data;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IProductUnitRepository : IRepository<ProductUnits>
    {
        Task<Brand> CreateAsync(ProductUnits  productUnits);
        Task<Brand> GetUnitsById(int Id);
        Task<IEnumerable<ProductUnits>> GetAllUnitsAsync();
    }




}
