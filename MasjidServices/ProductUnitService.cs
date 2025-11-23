using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
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



        public async Task<ProductUnits> CreateUnitAsync(ProductUnitsDto unitdto)
        {
            try
            {
                var newUnit = new ProductUnits
                {
                    UnitId = GenerateUniqueId(),
                    UnitName = unitdto.UnitName,
                    UnitDescription = unitdto.UnitDescription,
                    UnitSegments = unitdto.UnitSegments,
                    UnitSegmentsRatio = unitdto.UnitSegmentsRatio,
                    IsActive = unitdto.IsActive,
                    BranchId = unitdto.BranchId,
                    CompanyId = unitdto.CompanyId
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

        private string GenerateUniqueId()
        {
            var unitid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper();

            //check the generated id in the database, if exists create new and check again 

            return unitid;
        }

                       
        
        public Task<ProductUnitsDto> GetUnitsById(int unitId , string branchId, string companyId)
        {
            throw new NotImplementedException();
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

    }
}
