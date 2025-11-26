using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductUnitService : IProductUnitRepository
    {
        private readonly MasjidDBContext _context;

        public ProductUnitService(MasjidDBContext context)
        {
            _context = context;
        }



        public async Task<ProductUnits> CreateUnitAsync(ProductUnitsDto unitDto)
        {
            try
            {
                var newUnit = new ProductUnits
                {
                    UnitId = GenerateUnitId(unitDto.CompanyId),
                    UnitName = unitDto.UnitName,
                    UnitDescription = unitDto.UnitDescription,
                    UnitSegments = unitDto.UnitSegments,
                    UnitSegmentsRatio = unitDto.UnitSegmentsRatio,
                    IsActive = unitDto.IsActive,
                    BranchId = unitDto.BranchId,
                    CompanyId = unitDto.CompanyId
                };
                _context.ProductUnits.Add(newUnit);
                await _context.SaveChangesAsync();

                return newUnit;

            }
            catch (Exception ex)
            {
                return new ProductUnits();
            }
        }

        private string GenerateUnitId(string companyCode)
        {
            var unitid = companyCode + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper();
            //check the generated id in the database, if exists create new and check again 
            return unitid;
        }

        public async Task<bool> UpdateUnitAsync(ProductUnitsDto unitdto)
        {
            try
            {

                var unit = await _context.ProductUnits.Where(b => b.UnitId == unitdto.UnitId && b.CompanyId == unitdto.CompanyId && b.BranchId == unitdto.BranchId).FirstOrDefaultAsync();
                if (unit == null)
                {
                    return false;
                }

                unit.UnitId = unitdto.UnitId;
                unit.UnitName = unitdto.UnitName;
                unit.UnitDescription = unitdto.UnitDescription;
                unit.UnitSegments = unitdto.UnitSegments;
                unit.UnitSegmentsRatio = unitdto.UnitSegmentsRatio;
                unit.IsActive = unitdto.IsActive;
                unit.BranchId = unitdto.BranchId;
                unit.CompanyId = unitdto.CompanyId;

                _context.ProductUnits.Update(unit);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string GenerateUniqueId(string companyCode)
        {
            var unitid = companyCode + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper();

            //check the generated id in the database, if exists create new and check again 

            return unitid;
        }



        public async Task<ProductUnitsDto?> GetUnitsById(string unitId, string branchId, string companyId)
        {
            try
            {
                var p = await _context.ProductUnits.Where(u => u.UnitId == unitId && u.BranchId == branchId && u.CompanyId == companyId).FirstOrDefaultAsync();
                if (p == null)
                {
                    return null;
                }

                return new ProductUnitsDto
                {
                    UnitId = p.UnitId,
                    UnitName = p.UnitName,
                    UnitDescription = p.UnitDescription,
                    UnitSegments = p.UnitSegments,
                    UnitSegmentsRatio = p.UnitSegmentsRatio,
                    IsActive = p.IsActive,
                    BranchId = p.BranchId,
                    CompanyId = p.CompanyId

                };
            }

            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<IEnumerable<ProductUnitsDto>> GetAllUnitsAsync(string branchId, string companyId)
        {
            try
            {
                IEnumerable<ProductUnits> units = await _context.ProductUnits.Where(b => b.BranchId == branchId && b.CompanyId == companyId).ToListAsync();
                IEnumerable<ProductUnitsDto> allUnits = units.Select(p => new ProductUnitsDto
                {
                    UnitId = p.UnitId,
                    UnitName = p.UnitName,
                    UnitDescription = p.UnitDescription,
                    UnitSegments = p.UnitSegments,
                    UnitSegmentsRatio = p.UnitSegmentsRatio,
                    IsActive = p.IsActive,
                    BranchId = p.BranchId,
                    CompanyId = p.CompanyId,


                });

                return allUnits;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteUnitAsync(string unitId, string branchId, string companyId)
        {
            try
            {
                var unit = await _context.ProductUnits.Where(b => b.UnitId == unitId && b.CompanyId == companyId && b.BranchId == branchId).FirstOrDefaultAsync();
                if (unit == null)
                {
                    return false;
                }
                _context.ProductUnits.Remove(unit);
                await _context.SaveChangesAsync();
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}