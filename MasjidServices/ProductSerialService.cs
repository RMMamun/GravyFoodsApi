using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductSerialService : IProductSerialRepository
    {

        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public ProductSerialService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<ApiResponse<bool>> AddProductSerialAsync(IEnumerable<ProductSerialDto> productSerials)
        {
            ApiResponse<bool> apiRes = new ApiResponse<bool>();

            try
            {
                IEnumerable<ProductSerial> productSerialList = productSerials.Select(ps => new ProductSerial
                {
                    ProductId = ps.ProductId,
                    SerialNumber = ps.SerialNumber,
                    ManufactureDate = ps.ManufactureDate,
                    ExpiryDate = ps.ExpiryDate,
                    SKU = ps.SKU,
                    IMEI1 = ps.IMEI1,
                    IMEI2 = ps.IMEI2,
                    PurchaseId = ps.PurchaseId,
                    PurchaseDate = ps.PurchaseDate,
                    SalesId = ps.SalesId,
                    SalesDate = ps.SalesDate,
                    PurchaseReturnDate = ps.PurchaseReturnDate,
                    PurchaseReturnId = ps.PurchaseReturnId,
                    SalesReturnDate = ps.SalesReturnDate,
                    SalesReturnId = ps.SalesReturnId,
                    StockStatus = ps.StockStatus,
                    StockHistory = ps.StockHistory,
                    WHId = ps.WHId,
                    BranchId = _tenant.BranchId,
                    CompanyId = _tenant.CompanyId,

                });

                await _context.ProductSerials.AddRangeAsync(productSerialList);
                await _context.SaveChangesAsync();

                apiRes.Success = true;
                apiRes.Message = "Serial added successfully";

                return apiRes;

            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error adding serial";
                apiRes.Errors = new List<string> { ex.Message };
                return apiRes;
            }
        }

        public Task<ApiResponse<bool>> DeleteProductSerialAsync(int id, string branchId, string companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<ProductSerial>> GetProductSerialBySerialNumberAsync(string serialNumber, string branchId, string companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IEnumerable<ProductSerial>>> GetProductSerialsByProductIdAsync(string productId, string branchId, string companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> IsSerialNumberExistsAsync(string serialNumber, string branchId, string companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateProductSerialAsync(ProductSerialDto productSerial)
        {
            throw new NotImplementedException();
        }
    }
}
