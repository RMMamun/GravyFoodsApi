using AutoMapper;
using Azure;
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using System;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductStockService : IProductStockRepository
    {
        private readonly MasjidDBContext _context;
        private readonly IMapper _mapper;
        private  readonly IProductUnitRepository _UnitRepo;
        private readonly IUnitConversionRepository _unitConvRepo;
        public ProductStockService(MasjidDBContext context, IMapper mapper, IProductUnitRepository UnitRepo, IUnitConversionRepository unitConvRepo)
        {
            _context = context;
            _mapper = mapper;
            _UnitRepo = UnitRepo;
            _unitConvRepo = unitConvRepo;
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

        public class SmallUnitQtyDto
        { 
            public string Unit { get ; set; } 
            public int Qty { get; set; }

        }

        public async Task<SmallUnitQtyDto> ConvertToSmallUnit(double Quantity, string Unit,string UnitId, string BranchId, string CompanyId)
        {
            try
            {
                var unitInfo = _UnitRepo.GetUnitsById(UnitId, BranchId, CompanyId).Result;
                if (unitInfo == null)
                {
                    //response.Success = false;
                    //response.Message = "Stock update failed!! Unit details not found for unit: " + Unit;
                    //return response;
                    SmallUnitQtyDto dto = new SmallUnitQtyDto();
                    dto.Unit = Unit;
                    dto.Qty = 0;
                    
                    return dto;
                }
                else
                {
                    //Convert quantity to small unit
                    string unitSegments = unitInfo.UnitSegments;
                    string unitSegmentsRatio = unitInfo.UnitSegmentsRatio;

                    string[] units = unitSegments.Split('\\');
                    //unitsegmantration values are like - 1\1000\1000\1000
                    double[] segments = unitSegmentsRatio.Split('\\')
                        .Select(s =>
                        {
                            double val = 1;
                            double.TryParse(s, out val);
                            return val;
                        }).ToArray();

                    string smallUnit = units[units.Length - 1];

                    double convert = await _unitConvRepo.Convert(Quantity,Unit, smallUnit, units, segments);

                    SmallUnitQtyDto dto = new SmallUnitQtyDto();
                    dto.Unit = smallUnit;
                    dto.Qty = (int)convert;
                    return dto;

                }
            }
            catch (Exception ex)
            {
                SmallUnitQtyDto dto = new SmallUnitQtyDto();
                dto.Unit = "";
                dto.Qty = 0;
                return dto;
            }
        }
        public async Task<APIResponseDto> UpdateProductStockAsync(bool isAdd, string ProductId, double Quantity, string Unit,string UnitId,string WHId, string BranchId, string CompanyId)
        {
            APIResponseDto response = new APIResponseDto();

            try
            {

                //convert to small unit before updating stock
                var result = await ConvertToSmallUnit(Quantity, Unit,UnitId, BranchId, CompanyId);

                int qty = result.Qty;
                string smallUnit = result.Unit;

                //Check the product stock exists
                var existingStock = _context.Set<ProductStock>()
                    .FirstOrDefault(ps => ps.ProductId == ProductId
                                       && ps.BranchId == BranchId
                                       && ps.CompanyId == CompanyId);


                //***** Create new stock entry ***********************
                if (existingStock == null)  
                {
                    
                    ProductStock newStock = new ProductStock
                    {
                        ProductId = ProductId,
                        BranchId = BranchId,
                        CompanyId = CompanyId,
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
    }
}
