using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IBrandRepository 
    { 
        Task<ApiResponse<BrandDto>> CreateAsync(BrandDto brand);
        Task<ApiResponse<BrandDto>> GetBrandById(int Id);
        Task<ApiResponse<IEnumerable<BrandDto>>> GetBrandsAsync();

        Task<ApiResponse<bool>> UpdateAsync(BrandDto brand);
        Task<ApiResponse<bool>> DeleteAsync(int Id);

    }

    

    
}
