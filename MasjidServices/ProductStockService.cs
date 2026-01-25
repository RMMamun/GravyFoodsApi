using AutoMapper;
using Azure;
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductStockService : IProductStockRepository
    {
        private readonly MasjidDBContext _context;
        private readonly IMapper _mapper;
        private  readonly IProductUnitRepository _UnitRepo;
        private readonly IUnitConversionRepository _unitConvRepo;
        private readonly ITenantContextRepository _tenant;
        public ProductStockService(MasjidDBContext context, IMapper mapper, IProductUnitRepository UnitRepo, IUnitConversionRepository unitConvRepo, ITenantContextRepository tenant)
        {
            _context = context;
            _mapper = mapper;
            _UnitRepo = UnitRepo;
            _unitConvRepo = unitConvRepo;
            _tenant = tenant;
        }

        public async Task<ApiResponse<IEnumerable<ProductStockDto>>> GetAllProductStockAsync(string branchId, string companyId)
        {
            var response = new ApiResponse<IEnumerable<ProductStockDto>>();
            try
            {

                var stocks = await _context.Set<ProductStock>()
                    .Where(ps => ps.BranchId == _tenant.BranchId
                              && ps.CompanyId == _tenant.CompanyId)
                    .ToListAsync();

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

                return (response);
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "An error occurred while retrieving product stocks.";
                response.Errors = new List<string> { ex.Message.ToString() };

                return (response);

            }
        }

        public async Task<ApiResponse<ProductStockDto>> GetProductStockByIdAsync(string productId, string branchId, string companyId)
        {
            var response = new ApiResponse<ProductStockDto>();

            try
            {
                
                var stock = await _context.Set<ProductStock>()
                    .FirstOrDefaultAsync(ps => ps.ProductId == productId
                                       && ps.BranchId == _tenant.BranchId
                                       && ps.CompanyId == _tenant.CompanyId);

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
                return (response);
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "An error occurred while retrieving product stock.";
                response.Errors = new List<string> { ex.Message.ToString() };

                return (response);


            }

        }

        

        public async Task<APIResponseDto> UpdateProductStockAsync(bool isAdd, string ProductId, double Quantity, string Unit,string UnitId,string WHId, string BranchId, string CompanyId)
        {
            string _branchId = _tenant.BranchId;
            string _companyId = _tenant.CompanyId;

            APIResponseDto response = new APIResponseDto();

            try
            {

                //convert to small unit before updating stock
                var result = await _unitConvRepo.ConvertToSmallUnit(Quantity, Unit,UnitId, _branchId, _companyId);

                int qty = result.Qty;
                string smallUnit = result.Unit;

                //Check the product stock exists
                var existingStock = _context.Set<ProductStock>()
                    .FirstOrDefault(ps => ps.ProductId == ProductId
                                       && ps.BranchId == _branchId
                                       && ps.CompanyId == _companyId);


                //***** Create new stock entry ***********************
                if (existingStock == null)  
                {
                    
                    ProductStock newStock = new ProductStock
                    {
                        ProductId = ProductId,
                        BranchId = _branchId,
                        CompanyId = _companyId,
                        Quantity = isAdd ? qty : -qty,
                        SmallUnit = smallUnit,
                        WHId = WHId
                    };

                    _context.Set<ProductStock>().Add(newStock);
                    _context.SaveChanges();

                    response.Success = true;
                    response.Message = "Product stock created successfully.";

                    return response;
                }

                //Update the existing stock
                else
                {
                    existingStock.Quantity += isAdd ? qty : -qty;


                    existingStock.SmallUnit = smallUnit;
                    existingStock.WHId = WHId;
                    _context.SaveChanges();

                    response.Success = true;
                    response.Message = "Product stock updated successfully.";
                    
                    return response;

                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while updating product stock." + Environment.NewLine + ex.Message.ToString();

                return response;
            }

        }


        public async Task<ApiResponse<IEnumerable<LowStockProductsDto>>> GetLowStockProductsAsync(int totalProducts, string branchId, string companyId)
        {
            string _branchId = _tenant.BranchId;
            string _companyId = _tenant.CompanyId;  

            var response = new ApiResponse<IEnumerable<LowStockProductsDto>>();

            try
            {

                totalProducts = totalProducts <= 0 ? 5 : totalProducts;

                var rawStocks = await _context.ProductStocks
                    .Include(s => s.Product)
                    .Include(w => w.Warehouse)
                    .Where(s => s.BranchId == _branchId && s.CompanyId == _companyId)
                    .OrderBy(s => s.Quantity)
                    .Take(totalProducts)
                    .ToListAsync();

                var stockDtos = new List<LowStockProductsDto>();

                foreach (var s in rawStocks)
                {
                    var unit = await _unitConvRepo.ConvertToShowingUnit(s.ProductId,s.Quantity,s.Product.DefaultUnit, _branchId, _companyId);

                    stockDtos.Add(new LowStockProductsDto
                    {
                        ProductId = s.ProductId,
                        ProductName = s.Product.Name,
                        Quantity = unit.Qty,
                        ShowingUnit = unit.Unit,
                        WHId = s.WHId,
                        WHName = s.Warehouse.WHName,
                        BranchId = "",
                        CompanyId = ""
                    });
                }



                response.Data = stockDtos;
                response.Success = true;
                response.Message = "Product stocks retrieved successfully.";

                return response;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "An error occurred while retrieving product stocks.";
                response.Errors = new List<string> { ex.Message.ToString() };

                return response;

            }
        }

    }
}
