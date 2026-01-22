using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using GravyFoodsApi.Data;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IBrandRepository 
    { 
        Task<BrandDto> CreateAsync(BrandDto brand);
        Task<BrandDto> GetBrandById(int Id);
        Task<IEnumerable<BrandDto>> GetBrandsAsync();
    }

    

    
}
