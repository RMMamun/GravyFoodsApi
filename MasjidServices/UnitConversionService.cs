using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GravyFoodsApi.MasjidServices
{

    public class UnitConversionService : IUnitConversionRepository
    {
        private readonly MasjidDBContext _context;
        private readonly IProductUnitRepository _UnitRepo;
        public UnitConversionService(MasjidDBContext context, IProductUnitRepository UnitRepo)
        {
            _context = context;
            _UnitRepo = UnitRepo;


        }

              

        public async Task<double> ToBase(double value, int fromIndex, double[] segments)
        {
            double result = value;

            // multiply downward
            for (int i = fromIndex + 1; i < segments.Length; i++)
                result *= segments[i];

            return result;
        }


        public async Task<double> FromBase(double baseValue, int toIndex, double[] segments)
        {
            double result = baseValue;

            // divide upward
            for (int i = toIndex + 1; i < segments.Length; i++)
                result /= segments[i];

            return result;
        }



        public async Task<double> Convert(double value, string fromUnit, string toUnit, string[] units, double[] segments)
        {
            int from = Array.IndexOf(units, fromUnit);
            int to = Array.IndexOf(units, toUnit);

            // 1. Convert from any unit → base unit (smallest)
            double baseValue = await ToBase(value, from, segments);

            // 2. Convert from base unit → desired unit
            
            return await FromBase(baseValue, to, segments);

        }




        public async Task<Qty_UnitDto> ConvertToSmallUnit(double Quantity, string Unit, string UnitId, string BranchId, string CompanyId)
        {
            try
            {
                var unitInfo = _UnitRepo.GetUnitsById(UnitId, BranchId, CompanyId).Result;
                if (unitInfo == null)
                {
                    //response.Success = false;
                    //response.Message = "Stock update failed!! Unit details not found for unit: " + Unit;
                    //return response;
                    Qty_UnitDto dto = new Qty_UnitDto();
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

                    double convert = await this.Convert(Quantity, Unit, smallUnit, units, segments);

                    Qty_UnitDto dto = new Qty_UnitDto();
                    dto.Unit = smallUnit;
                    dto.Qty = (int)convert;
                    return dto;

                }
            }
            catch (Exception ex)
            {
                Qty_UnitDto dto = new Qty_UnitDto();
                dto.Unit = "";
                dto.Qty = 0;
                return dto;
            }
        }

        public async Task<Qty_UnitDto> ConvertToShowingUnit(string ProductId, double Quantity, string Unit, string BranchId, string CompanyId)
        {
            try
            {

                var result = (
                            from p in _context.Product.Where(p => p.ProductId == ProductId && p.BranchId == BranchId && p.CompanyId == CompanyId)
                            join u in _context.ProductUnits on p.UnitId equals u.UnitId
                            select new
                            {
                                ProductId = p.ProductId,
                                UnitId = u.UnitId,
                                DefaultUnit = p.DefaultUnit,
                                UnitSegments = u.UnitSegments,
                                UnitSegmentsRatio = u.UnitSegmentsRatio
                            }).FirstOrDefaultAsync();



                string ShowingUnit = result.Result.DefaultUnit;

                string UnitId = result.Result.UnitId;
                //var unitInfo = _UnitRepo.GetUnitsById(UnitId, BranchId, CompanyId).Result;

                if (result == null)
                {
                    //response.Success = false;
                    //response.Message = "Stock update failed!! Unit details not found for unit: " + Unit;
                    //return response;
                    Qty_UnitDto dto = new Qty_UnitDto();
                    dto.Unit = Unit;
                    dto.Qty = 0;

                    return dto;
                }
                else
                {
                    //Convert quantity to small unit
                    string unitSegments = result.Result.UnitSegments;
                    string unitSegmentsRatio = result.Result.UnitSegmentsRatio;

                    string[] units = unitSegments.Split('\\');
                    //unitsegmantration values are like - 1\1000\1000\1000
                    double[] segments = unitSegmentsRatio.Split('\\')
                        .Select(s =>
                        {
                            double val = 1;
                            double.TryParse(s, out val);
                            return val;
                        }).ToArray();


                    double convert = await this.Convert(Quantity, Unit, ShowingUnit, units, segments);

                    Qty_UnitDto dto = new Qty_UnitDto();
                    dto.Unit = ShowingUnit;
                    dto.Qty = (int)convert;
                    return dto;

                }
            }
            catch (Exception ex)
            {
                Qty_UnitDto dto = new Qty_UnitDto();
                dto.Unit = "";
                dto.Qty = 0;
                return dto;
            }
        }


    }


}
