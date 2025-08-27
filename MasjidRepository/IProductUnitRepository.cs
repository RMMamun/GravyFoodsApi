using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using GravyFoodsApi.Data;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IProductUnitRepository : IRepository<ProductUnits>
    {
        Task<ProductUnits> CreateAsync(ProductUnits  productUnits);
        Task<ProductUnits> GetUnitsById(int Id);
        Task<IEnumerable<ProductUnits>> GetAllUnitsAsync();
    }




}
