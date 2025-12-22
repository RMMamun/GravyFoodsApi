using GravyFoodsApi.Data;
using GravyFoodsApi.DTOs;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidServices
{
    public class WarehouseService : IWarehouseRepository
    {
        private readonly MasjidDBContext _context;

        public WarehouseService(MasjidDBContext context)
        {
            _context = context;
        }

        public Task<ApiResponse<WarehouseDto>> CreateWarehouseAsync(WarehouseDto warehouseDto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WarehouseDto>> DeleteWarehouseAsync(string WHId, string branchId, string companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<WarehouseDto>>> GetAllWarehousesAsync(string branchId, string companyId)
        {
            try
            {
                var warehouses = _context.Warehouse.Where(w => w.BranchId == branchId && w.CompanyId == companyId).ToList();
                var warehouseDtos = warehouses.Select(w => new WarehouseDto
                {
                    WHId = w.WHId,
                    WHName = w.WHName,
                    WHLocationNo = w.WHLocationNo,
                    BranchId = w.BranchId,
                    CompanyId = w.CompanyId
                }
                ).ToList();

                var response = new ApiResponse<List<WarehouseDto>>
                {
                    Data = warehouseDtos,
                    Success = true,
                    Message = "Warehouses retrieved successfully.",
                    Errors = null
                };

                return response;
            }

            catch (Exception ex)
            {
                var response = new ApiResponse<List<WarehouseDto>>
                {
                    Data = null,
                    Success = false,
                    Message = "An error occurred while retrieving warehouses.",
                    Errors = new List<string> { ex.Message.ToString() }
                };
                return response;
            }
        }

        public Task<ApiResponse<WarehouseDto>> GetWarehouseByIdAsync(string WHId, string branchId, string companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WarehouseDto>> UpdateWarehouseAsync(WarehouseDto warehouseDto)
        {
            throw new NotImplementedException();
        }
    }
}
