using GravyFoodsApi.Data;
using GravyFoodsApi.DTOs;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
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

        public async Task<ApiResponse<WarehouseDto>> CreateWarehouseAsync(WarehouseDto warehouseDto)
        {
            ApiResponse < WarehouseDto > apiRes = new ApiResponse<WarehouseDto>();

            try
            {
                var isExisted = await GetWarehouseByIdAsync(warehouseDto.WHId, warehouseDto.BranchId, warehouseDto.CompanyId);
                if (isExisted.Success == true)
                {
                    apiRes.Success = false;
                    apiRes.Message = $"Warehouse with ID '{warehouseDto.WHId}' already exists.";
                    return apiRes;

                    //return ServiceResultWrapper<SupplierInfo>.Fail($"Supplier with '{supplierInfo.Email}' OR '{supplierInfo.PhoneNo}' already exists.");
                }

                Warehouse newWH = new Warehouse
                {

                    WHId = GenerateWHId(warehouseDto.CompanyId),
                    WHName = warehouseDto.WHName,
                    WHLocationNo = warehouseDto.WHLocationNo,
                    BranchId = warehouseDto.BranchId,
                    CompanyId = warehouseDto.CompanyId,

                };

                await _context.Warehouse.AddAsync(newWH);
                await _context.SaveChangesAsync();

                warehouseDto.WHId = newWH.WHId;
                apiRes.Data = warehouseDto;
                return apiRes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GenerateWHId(string companyId)
        {
            var random = new Random();
            var WHId = companyId + "-" + random.Next(1000, 9999).ToString();
            return WHId;
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

        public async Task<ApiResponse<WarehouseDto>> GetWarehouseByIdAsync(string WHId, string branchId, string companyId)
        {
            var response = new ApiResponse<WarehouseDto>();

            try
            {
                var WHDto = _context.Warehouse.Where(w => w.WHId == WHId && w.BranchId == branchId && w.CompanyId == companyId).FirstOrDefault();

                if (WHDto != null)
                {

                    // Map to DTO
                    var warehouseDto = new WarehouseDto
                    {
                        WHId = WHDto.WHId,
                        WHName = WHDto.WHName,
                        WHLocationNo = WHDto.WHLocationNo,
                        BranchId = WHDto.BranchId,
                        CompanyId = WHDto.CompanyId
                    };

                    response.Data = warehouseDto;
                    response.Success = true;
                    response.Message = "Warehouses retrieved successfully.";
                    response.Errors = null;
                    
                }

                return response;
            }

            catch (Exception ex)
            {

                    response.Data = null;
                    response.Success = false;
                    response.Message = "An error occurred while retrieving warehouses.";
                    response.Errors = new List<string> { ex.Message.ToString() };
                
                return response;
            }
        }

        public Task<ApiResponse<WarehouseDto>> UpdateWarehouseAsync(WarehouseDto warehouseDto)
        {
            throw new NotImplementedException();
        }
    }
}
