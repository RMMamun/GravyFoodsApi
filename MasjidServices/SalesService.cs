
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

        public SalesService(MasjidDBContext context)
        {
            _context = context;
        }

        
        public async Task<SalesInfoDto> CreateSaleAsync(SalesInfoDto saleDto)
        {
            try
            {
                string strSalesId = GenerateSalesId();
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
                        BranchId = d.BranchId,
                        CompanyId = d.CompanyId,


                        SalesId = strSalesId // Fix: Set the required SalesId property
                    }).ToList()
                };
                _context.SalesInfo.Add(sale);
                await _context.SaveChangesAsync();

                saleDto.SalesId = strSalesId;
                return saleDto;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error creating sale: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }

        private string GenerateSalesId()
        {
            string str = Guid.NewGuid().ToString("N").Substring(0, 30).ToUpper();
            //check if already exists, 
            var isExist = _context.SalesInfo.Any(c => c.SalesId == str);
            if (isExist)
            {
                //recursively call the function until a unique ID is found
                return GenerateSalesId();
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

        public async Task<SalesInfo?> GetSaleByIdAsync(string salesId)
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
            .FirstOrDefaultAsync(s => s.SalesId == salesId);


        }


    }

}
