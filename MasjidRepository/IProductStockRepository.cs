using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using System;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IProductStockRepository 
    {
        Task<ProductStockDto> GetProductStockByIdAsync(string productId,string branchId, string companyId);
        Task<IEnumerable<ProductStockDto>> GetAllProductStockAsync(string branchId, string companyId);

        Task<ApiResponse<ProductStockDto>> UpdateProductStockAsync(ProductStockDto stockDto);

    }
}
