using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IProductCategoryRepository
    {
        Task<ApiResponse<bool>> CreateCategoryAsync(ProductCategoryDto productCategory);
        Task<ApiResponse<ProductCategoryDto?>> GetCategoryById(int Id);
        Task<ApiResponse<IEnumerable<ProductCategoryDto>>> GetAllCategoriesAsync();
    }

}
