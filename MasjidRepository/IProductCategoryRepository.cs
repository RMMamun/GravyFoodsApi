using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using GravyFoodsApi.Data;
using System;

namespace GravyFoodsApi.Repositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task<ProductCategory> CreateCategoryAsync(ProductCategory productCategory);
        Task<ProductCategory> GetCategoryById(int Id);
        Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync();
    }

}
