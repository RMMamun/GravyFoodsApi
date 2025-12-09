
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;


namespace GravyFoodsApi.MasjidServices
{

    public class PurchaseService : IPurchaseRepository
    {
        private readonly MasjidDBContext _context;
        private readonly IProductStockRepository _StockRepo;

        public PurchaseService(MasjidDBContext context, IProductStockRepository stockRepo   )
        {
            _context = context;
            _StockRepo = stockRepo;
        }


        public async Task<ApiResponse<PurchaseInfoDto>> CreatePurchaseAsync(PurchaseInfoDto PurDto)
        {

            ApiResponse<PurchaseInfoDto> apiRes = new ApiResponse<PurchaseInfoDto>();

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
                        string strPurchaseId = GeneratePurchaseId(PurDto.CompanyId);
                        PurchaseInfo Purchase = new PurchaseInfo
                        {
                            PurchaseId = strPurchaseId,
                            SupplierId = PurDto.SupplierId,
                            TotalAmount = PurDto.TotalAmount,
                            PaidAmount = PurDto.PaidAmount,
                            DueAmount = PurDto.DueAmount,
                            CreatedDateTime = DateTime.Now,
                            BranchId = PurDto.BranchId,
                            CompanyId = PurDto.CompanyId,
                            UserId = PurDto.UserId,
                            EditDateTime = null,
                            PaymentMethod = "",
                            PurchaseDate = PurDto.CreatedDateTime,
                            TransactionId = "",



                            PurchaseDetails = PurDto.PurchaseDetails.Select(d => new PurchaseDetails
                            {
                                ProductId = d.ProductId,
                                Quantity = d.Quantity,
                                UnitType = d.UnitType,
                                UnitId = d.UnitId,
                                WHId = d.WHId,
                                PricePerUnit = d.PricePerUnit,
                                TotalPrice = d.TotalPrice,
                                VATPerUnit = d.VATPerUnit,
                                TotalVAT = d.TotalVAT,
                                BranchId = PurDto.BranchId,
                                CompanyId = PurDto.CompanyId,


                                PurchaseId = strPurchaseId // Fix: Set the required PurchaseId property
                            }).ToList()
                        }; ;
                        _context.PurchaseInfo.Add(Purchase);
                        await _context.SaveChangesAsync();

                        //Update Stock after creating Sale
                        var stockUpdate = await StockUpdate(PurDto);
                        if (!stockUpdate.Success)
                        {

                            apiRes.Success = false;
                            apiRes.Message = "Sale created but stock update failed: " + stockUpdate.Message;

                            await transaction.RollbackAsync();
                            return apiRes;
                        }


                        await transaction.CommitAsync();


                        PurDto.PurchaseId = strPurchaseId;

                        apiRes.Success = true;
                        apiRes.Message = "Purchase created successfully.";
                        apiRes.Data = PurDto;

                        return apiRes;
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if any error occurs
                        await transaction.RollbackAsync();
                        throw; // Re-throw the exception to be handled by outer catch
                    }
                });
                

                return apiRes;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                //Console.WriteLine($"Error creating Purchase: {ex.Message}");
                //throw; // Re-throw the exception after logging it

                apiRes.Success = false;
                apiRes.Message = "Error creating Purchase: " + ex.Message;

                return apiRes;
            }
        }

        private async Task<APIResponseDto> StockUpdate(PurchaseInfoDto purDto)
        {
            APIResponseDto response = new APIResponseDto();

            try
            {
                ProductStockDto stock = new ProductStockDto();
                foreach (var item in purDto.PurchaseDetails)
                {

                    response = await _StockRepo.UpdateProductStockAsync(true, item.ProductId, item.Quantity, item.UnitType, item.UnitId, item.WHId, item.BranchId, item.CompanyId);
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

        private string GeneratePurchaseId(string companyCode)
        {
            string str = companyCode + Guid.NewGuid().ToString("N").Substring(0, 30).ToUpper();
            //check if already exists, 
            var isExist = _context.PurchaseInfo.Any(c => c.PurchaseId == str);
            if (isExist)
            {
                //recursively call the function until a unique ID is found
                return GeneratePurchaseId(companyCode);
            }
            return str;
        }

        //public async Task<PurchaseInfo> CreatePurchaseAsync(PurchaseInfoDto PurDto)
        //{

        //    PurchaseInfo Purchase = new PurchaseInfo
        //    {
        //        PurchaseId = PurDto.PurchaseId,
        //        SupplierId = PurDto.SupplierId,
        //        OrderStatus = PurDto.OrderStatus,
        //        TotalAmount = PurDto.TotalAmount,
        //        TotalDiscountAmount = PurDto.TotalDiscountAmount,
        //        TotalPaidAmount = PurDto.TotalPaidAmount,
        //        CreatedDateTime = DateTime.UtcNow,
        //        PurchaseDetails = PurDto.PurchaseDetails.Select(d => new PurchaseDetails
        //        {
        //            ProductId = d.ProductId,
        //            Quantity = d.Quantity,
        //            UnitType = d.UnitType,
        //            PricePerUnit = d.PricePerUnit,
        //            DiscountPerUnit = d.DiscountPerUnit,
        //            DiscountType = d.DiscountType
        //        }).ToList()
        //    };
        //    _context.PurchaseInfo.Add(Purchase);
        //    await _context.SaveChangesAsync();
        //    return Purchase;
        //}


        public async Task<PurchaseInfo?> UpdatePurchaseAsync(string PurchaseId, PurchaseInfo Purchase)
        {
            var existing = await _context.PurchaseInfo
                .Include(s => s.PurchaseDetails)
                .FirstOrDefaultAsync(s => s.PurchaseId == PurchaseId);

            if (existing == null)
                return null;

            // Update master fields
            _context.Entry(existing).CurrentValues.SetValues(Purchase);

            // Replace details if provided
            if (Purchase.PurchaseDetails?.Any() == true)
            {
                _context.PurchaseDetails.RemoveRange(existing.PurchaseDetails);
                existing.PurchaseDetails = Purchase.PurchaseDetails;
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeletePurchaseAsync(string PurchaseId, string BranchId, string CompanyId)
        {
            var existing = await _context.PurchaseInfo
                .Include(s => s.PurchaseDetails)
                .FirstOrDefaultAsync(s => s.PurchaseId == PurchaseId && s.BranchId == BranchId && s.CompanyId == CompanyId);

            if (existing == null)
                return false;

            _context.PurchaseDetails.RemoveRange(existing.PurchaseDetails);
            _context.PurchaseInfo.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<PurchaseInfoDto>> GetPurchaseByDateRangeAsync(string searchStr, DateTime fromDate, DateTime toDate, string branchId, string companyId)
        {
            try
            {

                var query = _context.PurchaseInfo
                    .Include(s => s.PurchaseDetails)
                    .Include(s => s.SupplierInfo)
                    .AsQueryable();

                query = query.Where(s => s.BranchId == branchId && s.CompanyId == companyId);

                // Date filter (only if provided)
                if (fromDate != null && toDate != null)
                {
                    query = query.Where(s =>
                        s.CreatedDateTime.Date >= fromDate.Date &&
                        s.CreatedDateTime.Date <= toDate.Date);
                }


                // Phrase search (only if not empty)
                if (!string.IsNullOrWhiteSpace(searchStr))
                {
                    query = query.Where(s =>
                        (s.SupplierInfo.SupplierName + " "
                        + s.PurchaseId + " "
                        + s.UserId)
                        .Contains(searchStr)
                    );
                }

                // Final projection
                var purchaseDtos = await query
                    .Select(s => new PurchaseInfoDto
                    {
                        PurchaseId = s.PurchaseId.ToString(),
                        SupplierId = s.SupplierId.ToString(),
                        SupplierName = s.SupplierInfo.SupplierName,
                        UserId = "",
                        BranchId = "",
                        CompanyId = "",
                        TotalAmount = s.TotalAmount,
                        CreatedDateTime = s.CreatedDateTime,

                        PurchaseDetails = s.PurchaseDetails.Select(d => new PurchaseDetailDto
                        {
                            ProductId = d.ProductId.ToString(),
                            ProductName = d.Product.Name,
                            Quantity = d.Quantity,
                            UnitType = d.UnitType,
                            UnitId = d.UnitId,
                            WHId = d.WHId,
                            PricePerUnit = d.PricePerUnit,
                            TotalPrice = d.TotalPrice,
                            VATPerUnit = d.VATPerUnit,
                            TotalVAT = d.TotalVAT,
                            UserId = "",
                            BranchId = "",
                            CompanyId = "",
                        }).ToList()
                    })
                    .ToListAsync();

                return purchaseDtos;

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error retrieving Purchase: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }

        }

        //public async Task<IEnumerable<PurchaseInfoDto>> GetPurchaseByDateRangeAsync(string searchStr, DateTime fromDate, DateTime toDate, string branchId, string companyId)
        //{
        //    try
        //    {


        //        //IEnumerable<PurchaseInfo> Purchase = await _context.PurchaseInfo
        //        //    .Include(s => s.PurchaseDetails)
        //        //    .Include(s => s.SupplierInfo)
        //        //    .ToListAsync();


        //        var PurchaseDtos = await _context.PurchaseInfo.Where(s => 
        //        s.CreatedDateTime.Date >= fromDate.Date && s.CreatedDateTime.Date <= toDate.Date && s.BranchId == branchId && s.CompanyId == companyId)
        //        .Include(s => s.PurchaseDetails)
        //        .Include(s => s.SupplierInfo)
        //        .Select(s => new PurchaseInfoDto
        //        {
        //            PurchaseId = s.PurchaseId.ToString(),   // adjust if Id is string
        //            SupplierId = s.SupplierId.ToString(),
        //            SupplierName = s.SupplierInfo.SupplierName,
        //            UserId = "", //s.UserId.ToString(),
        //            BranchId = "", //s.BranchId.ToString(),
        //            CompanyId = "", // s.CompanyId.ToString(),

        //            TotalAmount = s.TotalAmount,
        //            CreatedDateTime = s.CreatedDateTime,


        //            PurchaseDetails = s.PurchaseDetails.Select(d => new PurchaseDetailDto
        //            {
        //                ProductId = d.ProductId.ToString(),
        //                ProductName = d.Product.Name,   // assumes navigation to Product
        //                Quantity = d.Quantity,
        //                UnitType = d.UnitType,
        //                UnitId = d.UnitId,
        //                WHId = d.WHId,
        //                PricePerUnit = d.PricePerUnit,
        //                TotalPrice = d.TotalPrice,
        //                VATPerUnit = d.VATPerUnit,
        //                TotalVAT = d.TotalVAT,
        //                UserId = "", //d.UserId.ToString(),
        //                BranchId = "", //d.BranchId.ToString(),
        //                CompanyId = "", //d.CompanyId.ToString(),

        //            }).ToList()
        //        })
        //        .ToListAsync();


        //        //(s.SupplierInfo.SupplierName + " " + s.PurchaseId + " " + s.UserId).Contains(searchStr) && 
        //        PurchaseDtos = PurchaseDtos.Where(s => (s.SupplierName + " " + s.PurchaseId + " " + s.UserId).Contains(searchStr));

        //        return PurchaseDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (you can use a logging framework here)
        //        Console.WriteLine($"Error retrieving Purchase: {ex.Message}");
        //        throw; // Re-throw the exception after logging it
        //    }

        //}

        public async Task<IEnumerable<PurchaseInfoDto>> GetAllPurchaseAsync(string branchId, string companyId)
        {
            try
            {


                //IEnumerable<PurchaseInfo> Purchase = await _context.PurchaseInfo
                //    .Include(s => s.PurchaseDetails)
                //    .Include(s => s.SupplierInfo)
                //    .ToListAsync();


                var PurchaseDtos = await _context.PurchaseInfo
                .Include(s => s.PurchaseDetails)
                .Include(s => s.SupplierInfo)
                .Select(s => new PurchaseInfoDto
                {
                    PurchaseId = s.PurchaseId.ToString(),   // adjust if Id is string
                    SupplierId = s.SupplierId.ToString(),
                    SupplierName = s.SupplierInfo.SupplierName,
                    UserId = s.UserId.ToString(),
                    BranchId = s.BranchId.ToString(),
                    CompanyId = s.CompanyId.ToString(),

                    TotalAmount = s.TotalAmount,
                    CreatedDateTime = s.CreatedDateTime,


                    PurchaseDetails = s.PurchaseDetails.Select(d => new PurchaseDetailDto
                    {
                        ProductId = d.ProductId.ToString(),
                        ProductName = d.Product.Name,   // assumes navigation to Product
                        Quantity = d.Quantity,
                        UnitType = d.UnitType,
                        UnitId = d.UnitId,
                        WHId = d.WHId,
                        PricePerUnit = d.PricePerUnit,
                        TotalPrice = d.TotalPrice,
                        VATPerUnit = d.VATPerUnit,
                        TotalVAT = d.TotalVAT,
                        UserId = "", //d.UserId.ToString(),
                        BranchId = d.BranchId.ToString(),
                        CompanyId = d.CompanyId.ToString(),

                    }).ToList()
                })
                .ToListAsync();




                return PurchaseDtos;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error retrieving Purchase: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }

        }

        public async Task<PurchaseInfo?> GetPurchaseByIdAsync(string PurchaseId, string BranchId, string CompanyId)
        {
            //return await _context.PurchaseInfo
            //    .Include(s => s.PurchaseDetails)
            //    .ThenInclude(d => d.Product)
            //    .Include(s => s.SupplierInfo)

            //    .FirstOrDefaultAsync(s => s.PurchaseId == PurchaseId);

            return await _context.PurchaseInfo
                .Include(s => s.SupplierInfo)            // load Supplier
            .Include(s => s.PurchaseDetails)            // load Purchase details
                .ThenInclude(d => d.Product)         // load product for each detail
            .FirstOrDefaultAsync(s => s.PurchaseId == PurchaseId && s.BranchId == BranchId && s.CompanyId == CompanyId);


        }


    }

}
