using GravyFoodsApi.DTOs;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IWarehouseRepository
    {
        Task<ApiResponse<List<WarehouseDto>>> GetAllWarehousesAsync(string branchId, string companyId);
        Task<ApiResponse<WarehouseDto>> GetWarehouseByIdAsync(string WHId, string branchId, string companyId);
        Task<ApiResponse<WarehouseDto>> CreateWarehouseAsync(WarehouseDto warehouseDto);
        Task<ApiResponse<WarehouseDto>> UpdateWarehouseAsync(WarehouseDto warehouseDto);
        Task<ApiResponse<WarehouseDto>> DeleteWarehouseAsync(string WHId, string branchId, string companyId);
    }
}
