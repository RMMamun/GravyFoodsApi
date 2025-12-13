using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using System;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IProductSerialRepository
    {

        Task<ApiResponse<IEnumerable<ProductSerial>>> GetProductSerialsByProductIdAsync(string productId, string branchId, string companyId);
        Task<ApiResponse<bool>> AddProductSerialAsync(IEnumerable<ProductSerialDto> productSerials);

        Task<ApiResponse<ProductSerial>> GetProductSerialBySerialNumberAsync(string serialNumber, string branchId, string companyId);
        Task<ApiResponse<bool>> IsSerialNumberExistsAsync(string serialNumber, string branchId, string companyId);
      
        Task<ApiResponse<bool>> UpdateProductSerialAsync(ProductSerialDto productSerial);
        Task<ApiResponse<bool>> DeleteProductSerialAsync(int id, string branchId, string companyId);


    }

    
}
