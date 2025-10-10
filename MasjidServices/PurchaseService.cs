
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

        public PurchaseService(MasjidDBContext context)
        {
            _context = context;
        }


        public async Task<PurchaseInfoDto> CreatePurchaseAsync(PurchaseInfoDto PurchaseDto)
        {
            try
            {
                string strPurchaseId = GeneratePurchaseId();
                PurchaseInfo Purchase = new PurchaseInfo
                {
                    PurchaseId = strPurchaseId,
                    SupplierId = PurchaseDto.SupplierId,
                    TotalAmount = PurchaseDto.TotalAmount,
                    CreatedDateTime = PurchaseDto.CreatedDateTime,
                    BranchId = PurchaseDto.BranchId,
                    CompanyId = PurchaseDto.CompanyId,
                    UserId = PurchaseDto.UserId,


                    PurchaseDetails = PurchaseDto.PurchaseDetails.Select(d => new PurchaseDetails
                    {
                        ProductId = d.ProductId,
                        Quantity = d.Quantity,
                        UnitType = d.UnitType,
                        PricePerUnit = d.PricePerUnit,
                        TotalPrice = d.TotalPrice,
                        VATPerUnit = d.VATPerUnit,
                        TotalVAT = d.TotalVAT,
                        BranchId = d.BranchId,
                        CompanyId = d.CompanyId,


                        PurchaseId = strPurchaseId // Fix: Set the required PurchaseId property
                    }).ToList()
                };
                _context.PurchaseInfo.Add(Purchase);
                await _context.SaveChangesAsync();

                PurchaseDto.PurchaseId = strPurchaseId;
                return PurchaseDto;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error creating Purchase: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }

        private string GeneratePurchaseId()
        {
            string str = Guid.NewGuid().ToString("N").Substring(0, 30).ToUpper();
            //check if already exists, 
            var isExist = _context.PurchaseInfo.Any(c => c.PurchaseId == str);
            if (isExist)
            {
                //recursively call the function until a unique ID is found
                return GeneratePurchaseId();
            }
            return str;
        }

        //public async Task<PurchaseInfo> CreatePurchaseAsync(PurchaseInfoDto PurchaseDto)
        //{

        //    PurchaseInfo Purchase = new PurchaseInfo
        //    {
        //        PurchaseId = PurchaseDto.PurchaseId,
        //        SupplierId = PurchaseDto.SupplierId,
        //        OrderStatus = PurchaseDto.OrderStatus,
        //        TotalAmount = PurchaseDto.TotalAmount,
        //        TotalDiscountAmount = PurchaseDto.TotalDiscountAmount,
        //        TotalPaidAmount = PurchaseDto.TotalPaidAmount,
        //        CreatedDateTime = DateTime.UtcNow,
        //        PurchaseDetails = PurchaseDto.PurchaseDetails.Select(d => new PurchaseDetails
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

        public async Task<IEnumerable<PurchaseInfoDto>> GetPurchaseByDateRangeAsync(DateTime fromDate, DateTime toDate, string branchId, string companyId)
        {
            try
            {


                //IEnumerable<PurchaseInfo> Purchase = await _context.PurchaseInfo
                //    .Include(s => s.PurchaseDetails)
                //    .Include(s => s.SupplierInfo)
                //    .ToListAsync();


                var PurchaseDtos = await _context.PurchaseInfo.Where(s => s.CreatedDateTime.Date >= fromDate.Date && s.CreatedDateTime.Date <= toDate.Date && s.BranchId == branchId && s.CompanyId == companyId)
                .Include(s => s.PurchaseDetails)
                .Include(s => s.SupplierInfo)
                .Select(s => new PurchaseInfoDto
                {
                    PurchaseId = s.PurchaseId.ToString(),   // adjust if Id is string
                    SupplierId = s.SupplierId.ToString(),
                    SupplierName = s.SupplierInfo.SupplierName,
                    UserId = "", //s.UserId.ToString(),
                    BranchId = "", //s.BranchId.ToString(),
                    CompanyId = "", // s.CompanyId.ToString(),

                    TotalAmount = s.TotalAmount,
                    CreatedDateTime = s.CreatedDateTime,


                    PurchaseDetails = s.PurchaseDetails.Select(d => new PurchaseDetailDto
                    {
                        ProductId = d.ProductId.ToString(),
                        ProductName = d.Product.Name,   // assumes navigation to Product
                        Quantity = d.Quantity,
                        UnitType = d.UnitType,
                        PricePerUnit = d.PricePerUnit,
                        TotalPrice = d.TotalPrice,
                        VATPerUnit = d.VATPerUnit,
                        TotalVAT = d.TotalVAT,
                        UserId = "", //d.UserId.ToString(),
                        BranchId = "", //d.BranchId.ToString(),
                        CompanyId = "", //d.CompanyId.ToString(),

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
