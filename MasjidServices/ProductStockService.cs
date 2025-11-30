using AutoMapper;
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductStockService : IProductStockRepository
    {
        private readonly MasjidDBContext _context;
        private readonly IMapper _mapper;
        public ProductStockService(MasjidDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<ApiResponse<IEnumerable<ProductStockDto>>> GetAllProductStockAsync(string branchId, string companyId)
        {
            var response = new ApiResponse<IEnumerable<ProductStockDto>>();
            try
            {

                var stocks = _context.Set<ProductStock>()
                    .Where(ps => ps.BranchId == branchId
                              && ps.CompanyId == companyId)
                    .ToList();
                var stockDtos = stocks.Select(stock => new ProductStockDto
                {
                    ProductId = stock.ProductId,
                    BranchId = stock.BranchId,
                    CompanyId = stock.CompanyId,
                    Quantity = stock.Quantity,
                    SmallUnit = stock.SmallUnit,
                    WHId = stock.WHId
                }).ToList();

                response.Data = stockDtos;
                response.Success = true;
                response.Message = "Product stocks retrieved successfully.";

                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "An error occurred while retrieving product stocks.";
                response.Errors = new List<string> { ex.Message.ToString() };

                return Task.FromResult(response);

            }
        }

        public Task<ApiResponse<ProductStockDto>> GetProductStockByIdAsync(string productId, string branchId, string companyId)
        {
            var response = new ApiResponse<ProductStockDto>();

            try
            {
                
                var stock = _context.Set<ProductStock>()
                    .FirstOrDefault(ps => ps.ProductId == productId
                                       && ps.BranchId == branchId
                                       && ps.CompanyId == companyId);
                if (stock != null)
                    {
                    //var stockDto = new ProductStockDto
                    //{
                    //    ProductId = stock.ProductId,
                    //    BranchId = stock.BranchId,
                    //    CompanyId = stock.CompanyId,
                    //    Quantity = stock.Quantity,
                    //    SmallUnit = stock.SmallUnit,
                    //    WHId = stock.WHId
                    //};

                    //auto marge the data
                    
                    var stockDto = _mapper.Map<ProductStockDto>(stock);

                    response.Data = stockDto;
                    response.Success = true;
                    response.Message = "Product stock retrieved successfully.";
                }
                else
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = "Product stock not found.";
                }
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "An error occurred while retrieving product stock.";
                response.Errors = new List<string> { ex.Message.ToString() };

                return Task.FromResult(response);


            }

        }

        public Task<APIResponseDto> UpdateProductStockAsync(ProductStockDto stockDto)
        {
            APIResponseDto response = new APIResponseDto();

            try
            {
                

                //Check the product stock exists
                var existingStock = _context.Set<ProductStock>()
                    .FirstOrDefault(ps => ps.ProductId == stockDto.ProductId
                                       && ps.BranchId == stockDto.BranchId
                                       && ps.CompanyId == stockDto.CompanyId);


                if (existingStock == null)
                {
                    //***** Create new stock entry ***********************
                    ProductStock newStock = new ProductStock
                    {
                        ProductId = stockDto.ProductId,
                        BranchId = stockDto.BranchId,
                        CompanyId = stockDto.CompanyId,
                        Quantity = stockDto.Quantity,
                        SmallUnit = stockDto.SmallUnit,
                        WHId = stockDto.WHId
                    };

                    _context.Set<ProductStock>().Add(newStock);
                    _context.SaveChanges();

                    response.Success = true;
                    response.Message = "Product stock created successfully.";

                    return Task.FromResult(response);
                }

                else
                {
                    //Update the existing stock
                    existingStock.Quantity = existingStock.Quantity + stockDto.Quantity;
                    existingStock.SmallUnit = stockDto.SmallUnit;
                    existingStock.WHId = stockDto.WHId;
                    _context.SaveChanges();

                    response.Success = true;
                    response.Message = "Product stock updated successfully.";
                    
                    return Task.FromResult(response);

                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while updating product stock." + Environment.NewLine + ex.Message.ToString();

                throw;
            }

        }
    }
}
