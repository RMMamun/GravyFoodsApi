using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using GravyFoodsApi.Data;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IBrandRepository : IRepository<Brand> 
    { 
        Task<Brand> CreateAsync(Brand brand);
        Task<Brand> GetBrandById(int Id);
        Task<IEnumerable<Brand>> GetBrandsAsync();
    }

    

    
}
