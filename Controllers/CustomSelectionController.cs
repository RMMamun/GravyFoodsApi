using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomSelectionController : Controller
    {
        private readonly ICustomerInfoService _cusRepo;
        private readonly ISupplierRepository _suppRepo;
        private readonly IWarehouseRepository _wareRepo;


        public CustomSelectionController(ICustomerInfoService cusRepo, ISupplierRepository suppRepo, IWarehouseRepository wareRepo)
        {
            _cusRepo = cusRepo;
            _suppRepo = suppRepo;
            _wareRepo = wareRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppOptions([FromQuery] AppOptionAPIParameterDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.BranchId) || string.IsNullOrEmpty(request.CompanyId))
            {
                return BadRequest("Invalid request!");
            }



            switch (request.OptionName)
            {
                case "CUSTOMER":
                    return Ok(await GetCustomersAsync(request.BranchId, request.CompanyId));
                //break;
                case "SUPPLIER":
                    // Custom logic for INVENTORY option can be added here
                    return Ok(await GetSuppliersAsync(request.BranchId, request.CompanyId));
                case "WAREHOUSE":
                    // Custom logic for INVENTORY option can be added here
                    return Ok(await GetWarehouseAsync(request.BranchId, request.CompanyId));
                // Add more cases as needed
                default:
                    return NotFound();
            }





        }

        private async Task<ApiResponse<List<CustomSelectionListDto>>> GetCustomersAsync(string branchId, string companyId)
        {
            try
            {
                var res = await _cusRepo.GetAllCustomersAsync(branchId, companyId);

                var list = res.Select(x => new CustomSelectionListDto
                {
                    Id = x.CustomerId,
                    Name = x.CustomerName,
                    BranchId = x.BranchId,
                    CompanyId = x.CompanyId
                }).ToList();

                var response = new ApiResponse<List<CustomSelectionListDto>>
                {
                    Success = true,
                    Message = "Customers retrieved successfully.",
                    Data = list
                };

                return response;

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CustomSelectionListDto>>
                {
                    Success = false,
                    Message = "Error retrieving customers.",
                    Errors = new List<string> { ex.Message }
                };
            }


        }

        private async Task<ApiResponse<List<CustomSelectionListDto>>> GetSuppliersAsync(string branchId, string companyId)
        {
            try
            {
                var res = await _suppRepo.GetAllSuppliersAsync(branchId, companyId);
                var list = res.Select(x => new CustomSelectionListDto
                {
                    Id = x.SupplierId,
                    Name = x.SupplierName,
                    BranchId = x.BranchId,
                    CompanyId = x.CompanyId
                }).ToList();

                var response = new ApiResponse<List<CustomSelectionListDto>>
                {
                    Success = true,
                    Message = "Supplier retrieved successfully.",
                    Data = list
                };

                return response;

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CustomSelectionListDto>>
                {
                    Success = false,
                    Message = "Error retrieving Supplier.",
                    Errors = new List<string> { ex.Message }
                };
            }

        }


        private async Task<ApiResponse<List<CustomSelectionListDto>>> GetWarehouseAsync(string branchId, string companyId)
        {
            try
            {
                var res = await _wareRepo.GetAllWarehousesAsync(branchId, companyId);
                var list = res.Data.Select(x => new CustomSelectionListDto
                {
                    Id = x.WHId,
                    Name = x.WHName,
                    BranchId = x.BranchId,
                    CompanyId = x.CompanyId
                }).ToList();

                var response = new ApiResponse<List<CustomSelectionListDto>>
                {
                    Success = true,
                    Message = "Supplier retrieved successfully.",
                    Data = list
                };

                return response;

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CustomSelectionListDto>>
                {
                    Success = false,
                    Message = "Error retrieving Supplier.",
                    Errors = new List<string> { ex.Message }
                };
            }

        }


    }
}
