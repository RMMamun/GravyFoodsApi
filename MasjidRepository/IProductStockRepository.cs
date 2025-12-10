using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using System;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IProductStockRepository 
    {
        Task<ApiResponse<ProductStockDto>> GetProductStockByIdAsync(string productId,string branchId, string companyId);
        Task<ApiResponse<IEnumerable<ProductStockDto>>> GetAllProductStockAsync(string branchId, string companyId);

        Task<ApiResponse<IEnumerable<LowStockProductsDto>>> GetLowerStockProductAsync(int totalProducts, string branchId, string companyId);

        Task<APIResponseDto> UpdateProductStockAsync(bool isAdd,string ProductId, double Quantity, string Unit, string UnitId, string WHId, string BranchId, string CompanyId);

    }
}
