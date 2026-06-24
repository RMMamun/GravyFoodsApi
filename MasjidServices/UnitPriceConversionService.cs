using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace GravyFoodsApi.MasjidServices
{
    public class UnitPriceConversionService : IUnitPriceConversionRepository
    {
        private readonly MasjidDBContext _context;
        private readonly IProductUnitRepository _UnitRepo;
        private readonly IProductRepository _productRepo;
        private readonly ITenantContextRepository _tenant;
        public UnitPriceConversionService(MasjidDBContext context, IProductUnitRepository UnitRepo,IProductRepository product, ITenantContextRepository tenant)
        {
            _context = context;
            _UnitRepo = UnitRepo;
            _tenant = tenant;
            _productRepo = product;
        }


        public async Task<ApiResponse<List<ProductUnitsWithPriceDto>>> GetProductUnitsWithPriceAsync(string productId)
        {
            ApiResponse<List<ProductUnitsWithPriceDto>> apiRes = new ApiResponse<List<ProductUnitsWithPriceDto>>();
            try
            { 
                
                //var product = await _context.Product.FirstOrDefaultAsync(p => p.ProductId == productId && p.CompanyId == _tenant.CompanyId && p.BranchId == _tenant.BranchId);
                var _product = await _productRepo.GetProductByIdAsync(productId);
                if (_product == null || _product.Success == false)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Product not found.";
                    apiRes.Data = new List<ProductUnitsWithPriceDto>();
                    return apiRes;
                }
                var product = _product.Data;

                var _unit = await _UnitRepo.GetUnitByIdAsync(product.UnitId);
                if (_unit == null)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Unit not found.";
                    apiRes.Data = new List<ProductUnitsWithPriceDto>();
                    return apiRes;
                }
                var unit = _unit;

                //var Units = unit?.UnitSegments?.Split('\\').ToList() ?? new();


                List<ProductUnitsWithPriceDto> unitsWithPrice = new List<ProductUnitsWithPriceDto>();

                unitsWithPrice = GetUnitPrices(
                    productId,
                    product.DefaultUnit,
                    product.Price,
                    unit.UnitSegments,
                    unit.UnitSegmentsRatio
                );

                //return unitsWithPrice;
                apiRes.Success = true;
                apiRes.Message = "Units with price retrieved successfully.";
                apiRes.Data = unitsWithPrice;
                return apiRes;
            }
            catch (Exception ex)
            {
                //return new List<ProductUnitsWithPriceDto>();
                apiRes.Success = false;
                apiRes.Message = "Error retrieving units with price: " + ex.Message;
                apiRes.Data = new List<ProductUnitsWithPriceDto>();
                return apiRes;
            }
        }

        public static List<ProductUnitsWithPriceDto> GetUnitPrices(
                                                                string productId,
                                                                string assignedUnit,
                                                                decimal assignedUnitPrice,
                                                                string unitSegments,
                                                                string unitRatios)
        {
            try
            {
                var units = unitSegments.Split('\\');
                var ratios = unitRatios.Split('\\')
                                       .Select(decimal.Parse)
                                       .ToArray();

                if (units.Length != ratios.Length)
                    throw new Exception("UnitSegments and UnitRatios count mismatch.");

                // Quantity represented by 1 unit in terms of smallest unit
                var factors = new Dictionary<string, decimal>();

                decimal factor = 1;

                for (int i = units.Length - 1; i >= 0; i--)
                {
                    factors[units[i]] = factor;

                    if (i > 0)
                    {
                        factor *= ratios[i];
                    }
                }

                if (!factors.ContainsKey(assignedUnit))
                    throw new Exception($"Assigned unit '{assignedUnit}' not found.");

                decimal assignedFactor = factors[assignedUnit];

                var result = new List<ProductUnitsWithPriceDto>();

                foreach (var unit in units)
                {
                    decimal unitFactor = factors[unit];

                    decimal unitPrice =
                        assignedUnitPrice *
                        unitFactor /
                        assignedFactor;

                    result.Add(new ProductUnitsWithPriceDto
                    {
                        ProductId = productId,
                        UnitName = unit,
                        UnitPrice = Math.Round(unitPrice, 6)
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error calculating unit prices: " + ex.Message);
            }
        }


        //public static List<ProductUnitsWithPriceDto> GetUnitPrices(
        //                                                            string productId,
        //                                                            string assignedUnit,
        //                                                            decimal assignedUnitPrice,
        //                                                            string unitSegments,
        //                                                            string unitRatios)
        //{
        //    try
        //    {
        //        var units = unitSegments.Split('\\');
        //        var ratios = unitRatios.Split('\\')
        //                               .Select(decimal.Parse)
        //                               .ToArray();

        //        if (units.Length != ratios.Length)
        //            throw new Exception("UnitSegments and UnitRatios count mismatch.");

        //        // Build factor map relative to smallest unit
        //        var factors = new Dictionary<string, decimal>();

        //        decimal factor = 1;

        //        for (int i = units.Length - 1; i >= 0; i--)
        //        {
        //            factors[units[i]] = factor;

        //            if (i > 0)
        //                factor *= ratios[i];
        //        }

        //        if (!factors.ContainsKey(assignedUnit))
        //            throw new Exception($"Assigned unit '{assignedUnit}' not found.");

        //        decimal assignedFactor = factors[assignedUnit];

        //        var result = new List<ProductUnitsWithPriceDto>();

        //        foreach (var unit in units)
        //        {
        //            decimal unitFactor = factors[unit];

        //            decimal unitPrice =
        //                assignedUnitPrice *
        //                assignedFactor /
        //                unitFactor;

        //            result.Add(new ProductUnitsWithPriceDto
        //            {
        //                ProductId = productId,
        //                UnitName = unit,
        //                UnitPrice = Math.Round(unitPrice, 6)
        //            });
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions as needed
        //        throw new Exception("Error calculating unit prices: " + ex.Message);
        //    }
        //}


    }
}
