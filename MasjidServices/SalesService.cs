
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;


namespace GravyFoodsApi.MasjidServices
{

    public class SalesService : ISalesService
    {
        private readonly MasjidDBContext _context;
        private readonly IProductStockRepository _StockRepo;


        public SalesService(MasjidDBContext context, IProductStockRepository stockRepo)
        {
            _context = context;
            _StockRepo = stockRepo;
        }


        public async Task<ApiResponse<SalesInfoDto>> CreateSalesAsync(SalesInfoDto saleDto)
        {
            ApiResponse<SalesInfoDto> apiRes = new ApiResponse<SalesInfoDto>();

            // Get execution strategy (required for SQL Azure / retry logic)
            var strategy = _context.Database.CreateExecutionStrategy();

            try
            {
                await strategy.ExecuteAsync(async () =>
                {
                    // START TRANSACTION INSIDE RETRY STRATEGY
                    await using var transaction = await _context.Database.BeginTransactionAsync();

                    try
                    {
                        string strSalesId = GenerateSalesId(saleDto.CompanyId);


                        SalesInfo sale = new SalesInfo
                        {
                            SalesId = strSalesId,
                            CustomerId = saleDto.CustomerId,
                            OrderStatus = saleDto.OrderStatus,
                            TotalAmount = saleDto.TotalAmount,
                            TotalDiscountAmount = saleDto.TotalDiscountAmount,
                            TotalPaidAmount = saleDto.TotalPaidAmount,
                            CreatedDateTime = saleDto.CreatedDateTime,
                            BranchId = saleDto.BranchId,
                            CompanyId = saleDto.CompanyId,
                            UserId = saleDto.UserId,


                            SalesDetails = saleDto.SalesDetails.Select(d => new SalesDetails
                            {
                                ProductId = d.ProductId,
                                Quantity = d.Quantity,
                                UnitType = d.UnitType,
                                PricePerUnit = d.PricePerUnit,
                                DiscountPerUnit = d.DiscountPerUnit,
                                DiscountType = d.DiscountType,
                                TotalDiscount = d.TotalDiscount,
                                TotalPrice = d.TotalPrice,
                                VATPerUnit = d.VATPerUnit,
                                TotalVAT = d.TotalVAT,
                                UnitId = d.UnitId,
                                WHId = d.WHId,
                                DiscountAmountPerUnit = 0,
                                
                                BranchId = d.BranchId,
                                CompanyId = d.CompanyId,


                                SalesId = strSalesId // Fix: Set the required SalesId property
                            }).ToList()
                        };
                        _context.SalesInfo.Add(sale);
                        await _context.SaveChangesAsync();


                        //Update Stock after creating Sale
                        var stockUpdate = await StockUpdate(saleDto);
                        if (!stockUpdate.Success)
                        {

                            apiRes.Success = false;
                            apiRes.Message = "Sale created but stock update failed: " + stockUpdate.Message;

                            await transaction.RollbackAsync();
                            return apiRes;
                        }


                        await transaction.CommitAsync();



                        saleDto.SalesId = strSalesId;

                        apiRes.Success = true;
                        apiRes.Data = saleDto;
                        apiRes.Message = "Sale created successfully.";

                        return apiRes;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw; // This will be caught by the outer catch
                        
                    }
                });

                
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                //Console.WriteLine($"Error creating sale: {ex.Message}");
                //throw; // Re-throw the exception after logging it
                apiRes.Success = false;
                apiRes.Message = "Error creating sale: " + ex.Message;

                return apiRes;

            }

            return apiRes;
        }

        private async Task<APIResponseDto> StockUpdate(SalesInfoDto sale)
        {
            APIResponseDto response = new APIResponseDto();

            try
            {
                ProductStockDto stock = new ProductStockDto();
                foreach (var item in sale.SalesDetails)
                {

                    response = await _StockRepo.UpdateProductStockAsync(false,item.ProductId, item.Quantity, item.UnitType, item.UnitId, item.WHId, item.BranchId, item.CompanyId);
                }

                response.Success = true;
                response.Message = "Stock updated successfully.";
                return response;
                // Your stock update logic here

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while updating stock." + Environment.NewLine + ex.Message;
                return response;
            }
        }

        private string GenerateSalesId(string companyCode)
        {
            string str = companyCode + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
            //check if already exists, 
            var isExist = _context.SalesInfo.Any(c => c.SalesId == str);
            if (isExist)
            {
                //recursively call the function until a unique ID is found
                return GenerateSalesId(companyCode);
            }
            return str;
        }

        //public async Task<SalesInfo> CreateSaleAsync(SalesInfoDto saleDto)
        //{

        //    SalesInfo sale = new SalesInfo
        //    {
        //        SalesId = saleDto.SalesId,
        //        CustomerId = saleDto.CustomerId,
        //        OrderStatus = saleDto.OrderStatus,
        //        TotalAmount = saleDto.TotalAmount,
        //        TotalDiscountAmount = saleDto.TotalDiscountAmount,
        //        TotalPaidAmount = saleDto.TotalPaidAmount,
        //        CreatedDateTime = DateTime.UtcNow,
        //        SalesDetails = saleDto.SalesDetails.Select(d => new SalesDetails
        //        {
        //            ProductId = d.ProductId,
        //            Quantity = d.Quantity,
        //            UnitType = d.UnitType,
        //            PricePerUnit = d.PricePerUnit,
        //            DiscountPerUnit = d.DiscountPerUnit,
        //            DiscountType = d.DiscountType
        //        }).ToList()
        //    };
        //    _context.SalesInfo.Add(sale);
        //    await _context.SaveChangesAsync();
        //    return sale;
        //}


        public async Task<SalesInfo?> UpdateSaleAsync(string salesId, SalesInfo sale)
        {
            var existing = await _context.SalesInfo
                .Include(s => s.SalesDetails)
                .FirstOrDefaultAsync(s => s.SalesId == salesId);

            if (existing == null)
                return null;

            // Update master fields
            _context.Entry(existing).CurrentValues.SetValues(sale);

            // Replace details if provided
            if (sale.SalesDetails?.Any() == true)
            {
                _context.SalesDetails.RemoveRange(existing.SalesDetails);
                existing.SalesDetails = sale.SalesDetails;
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteSaleAsync(string salesId)
        {
            var existing = await _context.SalesInfo
                .Include(s => s.SalesDetails)
                .FirstOrDefaultAsync(s => s.SalesId == salesId);

            if (existing == null)
                return false;

            _context.SalesDetails.RemoveRange(existing.SalesDetails);
            _context.SalesInfo.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SalesInfoDto>> GetSalesByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId )
        {
            try
            {


                //IEnumerable<SalesInfo> sales = await _context.SalesInfo
                //    .Include(s => s.SalesDetails)
                //    .Include(s => s.CustomerInfo)
                //    .ToListAsync();


                var salesDtos = await _context.SalesInfo.Where(s => s.CreatedDateTime.Date >= fromDate.Date && s.CreatedDateTime.Date <= toDate.Date && s.BranchId == branchId && s.CompanyId == companyId)
                .Include(s => s.SalesDetails)
                .Include(s => s.CustomerInfo)
                .Select(s => new SalesInfoDto
                {
                    SalesId = s.SalesId.ToString(),   // adjust if Id is string
                    CustomerId = s.CustomerId.ToString(),
                    CustomerName = s.CustomerInfo.CustomerName,
                    UserId = "", //s.UserId.ToString(),
                    BranchId = "", //s.BranchId.ToString(),
                    CompanyId = "", // s.CompanyId.ToString(),

                    OrderStatus = s.OrderStatus,
                    TotalAmount = s.TotalAmount,
                    TotalDiscountAmount = s.TotalDiscountAmount,
                    TotalPaidAmount = s.TotalPaidAmount,
                    CreatedDateTime = s.CreatedDateTime,


                    SalesDetails = s.SalesDetails.Select(d => new SalesDetailDto
                    {
                        ProductId = d.ProductId.ToString(),
                        ProductName = d.Product.Name,   // assumes navigation to Product
                        Quantity = d.Quantity,
                        UnitType = d.UnitType,
                        UnitId = d.UnitId,
                        PricePerUnit = d.PricePerUnit,
                        DiscountPerUnit = d.DiscountPerUnit,
                        TotalPrice = d.TotalPrice,
                        TotalDiscount = d.TotalDiscount,
                        VATPerUnit = d.VATPerUnit,
                        TotalVAT = d.TotalVAT,
                        DiscountType = d.DiscountType,
                        UserId = "", //d.UserId.ToString(),
                        BranchId = "", //d.BranchId.ToString(),
                        CompanyId = "", //d.CompanyId.ToString(),

                    }).ToList()
                })
                .ToListAsync();




                return salesDtos;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error retrieving sales: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }

        }

        public async Task<IEnumerable<SalesInfoDto>> GetAllSalesAsync()
        {
            try
            {


                //IEnumerable<SalesInfo> sales = await _context.SalesInfo
                //    .Include(s => s.SalesDetails)
                //    .Include(s => s.CustomerInfo)
                //    .ToListAsync();


                var salesDtos = await _context.SalesInfo
                .Include(s => s.SalesDetails)
                .Include(s => s.CustomerInfo)
                .Select(s => new SalesInfoDto
                {
                    SalesId = s.SalesId.ToString(),   // adjust if Id is string
                    CustomerId = s.CustomerId.ToString(),
                    CustomerName = s.CustomerInfo.CustomerName,
                    UserId = "", //s.UserId.ToString(),
                    BranchId = "", //s.BranchId.ToString(),
                    CompanyId = "", // s.CompanyId.ToString(),

                    OrderStatus = s.OrderStatus,
                    TotalAmount = s.TotalAmount,
                    TotalDiscountAmount = s.TotalDiscountAmount,
                    TotalPaidAmount = s.TotalPaidAmount,
                    CreatedDateTime = s.CreatedDateTime,


                    SalesDetails = s.SalesDetails.Select(d => new SalesDetailDto
                    {
                        ProductId = d.ProductId.ToString(),
                        ProductName = d.Product.Name,   // assumes navigation to Product
                        Quantity = d.Quantity,
                        UnitType = d.UnitType,
                        UnitId = d.UnitId,
                        PricePerUnit = d.PricePerUnit,
                        DiscountPerUnit = d.DiscountPerUnit,
                        TotalPrice = d.TotalPrice,
                        TotalDiscount = d.TotalDiscount,
                        VATPerUnit = d.VATPerUnit,
                        TotalVAT = d.TotalVAT,
                        DiscountType = d.DiscountType,
                        UserId = "", //d.UserId.ToString(),
                        BranchId = "", //d.BranchId.ToString(),
                        CompanyId = "", //d.CompanyId.ToString(),

                    }).ToList()
                })
                .ToListAsync();




                return salesDtos;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error retrieving sales: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }

        }

        public async Task<ApiResponse<SalesInfoDto>> GetSaleByIdAsync(string salesId, string branchId, string companyId)
        {
            ApiResponse<SalesInfoDto> apiRes = new ApiResponse<SalesInfoDto>();

            try
            {
                var salesDtos = await _context.SalesInfo.Where (s => s.SalesId == salesId && s.BranchId == branchId && s.CompanyId == companyId)
                .Include(s => s.SalesDetails)
                .Include(s => s.CustomerInfo)
                .Select(s => new SalesInfoDto
                {
                    SalesId = s.SalesId.ToString(),   // adjust if Id is string
                    CustomerId = s.CustomerId.ToString(),
                    CustomerName = s.CustomerInfo.CustomerName,
                    UserId = s.UserId.ToString(),
                    BranchId = s.BranchId.ToString(),
                    CompanyId = s.CompanyId.ToString(),

                    OrderStatus = s.OrderStatus,
                    TotalAmount = s.TotalAmount,
                    TotalDiscountAmount = s.TotalDiscountAmount,
                    TotalPaidAmount = s.TotalPaidAmount,
                    CreatedDateTime = s.CreatedDateTime,
                    


                    SalesDetails = s.SalesDetails.Select(d => new SalesDetailDto
                    {
                        ProductId = d.ProductId.ToString(),
                        ProductName = d.Product.Name,   // assumes navigation to Product
                        Quantity = d.Quantity,
                        UnitType = d.UnitType,
                        UnitId = d.UnitId,
                        PricePerUnit = d.PricePerUnit,
                        DiscountPerUnit = d.DiscountPerUnit,
                        TotalPrice = d.TotalPrice,
                        TotalDiscount = d.TotalDiscount,
                        VATPerUnit = d.VATPerUnit,
                        TotalVAT = d.TotalVAT,
                        DiscountType = d.DiscountType,
                        UserId = s.UserId.ToString(),
                        BranchId = d.BranchId.ToString(),
                        CompanyId = d.CompanyId.ToString(),
                        WHId = d.WHId,
                        

                    }).ToList()
                }).FirstOrDefaultAsync();


                apiRes.Message = "Sale retrieved successfully.";
                apiRes.Success = true;
                apiRes.Data = salesDtos;

                return apiRes;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                //Console.WriteLine($"Error retrieving sale: {ex.Message}");
                //throw; // Re-throw the exception after logging it
                apiRes.Success = false;
                apiRes.Message = "Error retrieving sale: " + ex.Message;
                return apiRes;

            }

            //return await _context.SalesInfo
            //    .Include(s => s.SalesDetails)
            //    .ThenInclude(d => d.Product)
            //    .Include(s => s.CustomerInfo)

            //    .FirstOrDefaultAsync(s => s.SalesId == salesId);

            //return await _context.SalesInfo
            //    .Include(s => s.CustomerInfo)            // load customer
            //    .Include(s => s.SalesDetails)            // load sales details
            //    .ThenInclude(d => d.Product)         // load product for each detail
            //.FirstOrDefaultAsync(s => s.SalesId == salesId && s.BranchId == branchId && s.CompanyId == companyId);


        }


        public async Task<SalesInfo?> GetSaleInvoiceByIdAsync(string salesId, string branchId, string companyId)
        {
            try
            {

                //return await _context.SalesInfo
                //    .Include(s => s.SalesDetails)
                //    .ThenInclude(d => d.Product)
                //    .Include(s => s.CustomerInfo)

                //    .FirstOrDefaultAsync(s => s.SalesId == salesId);

                return await _context.SalesInfo
                    .Include(s => s.CustomerInfo)            // load customer
                    .Include(s => s.SalesDetails)            // load sales details
                    .ThenInclude(d => d.Product)         // load product for each detail
                .FirstOrDefaultAsync(s => s.SalesId == salesId && s.BranchId == branchId && s.CompanyId == companyId);



            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error retrieving sale: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }

        }


        public async Task<ApiResponse<IEnumerable<BestSoldProductsDto>>> GetBestSoldProductsByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId)
        {
            ApiResponse<IEnumerable<BestSoldProductsDto>> apiRes = new ApiResponse<IEnumerable<BestSoldProductsDto>>();

            try
            {


                var bestSoldProducts = await _context.SalesDetails
                .Where(d =>
                    d.SalesInfo.CreatedDateTime.Date >= fromDate.Date &&
                    d.SalesInfo.CreatedDateTime.Date <= toDate.Date &&
                    d.SalesInfo.BranchId == branchId &&
                    d.SalesInfo.CompanyId == companyId
                )
                .GroupBy(d => new
                {
                    d.ProductId,
                    d.Product.Name,
                    d.Product.ProductCode
                })
                .Select(g => new BestSoldProductsDto
                {
                    ProductId = g.Key.ProductId.ToString(),
                    ProductName = g.Key.Name,
                    ProductCode = g.Key.ProductCode,

                    TotalQuantitySold = (decimal)g.Sum(x => x.Quantity),
                    TotalSalesAmount = g.Sum(x => x.TotalPrice),
                    TotalDiscount = g.Sum(x => x.TotalDiscount),
                    TotalVAT = g.Sum(x => x.TotalVAT),
                    BranchId = branchId,
                    CompanyId = companyId
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .ToListAsync();

                apiRes.Data = bestSoldProducts;
                apiRes.Message = "Best sold products retrieved successfully.";
                apiRes.Success = true;

                return apiRes;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                //Console.WriteLine($"Error retrieving sales: {ex.Message}");
                //throw; // Re-throw the exception after logging it

                apiRes.Success = false;
                apiRes.Message = "Error retrieving best sold products: " + ex.Message;
                
                return apiRes;

            }

        }



    }

}
