using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using GravyFoodsApi.Data;
using System;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.Repositories
{
    public interface IProductUnitRepository 
    {
        Task<ProductUnits> CreateUnitAsync(ProductUnitsDto  productUnits);
        Task<ProductUnitsDto> GetUnitsById(int unitId, string branchId, string companyId);
        Task<IEnumerable<ProductUnitsDto>> GetAllUnitsAsync(string branchId, string companyId);
    }




}
