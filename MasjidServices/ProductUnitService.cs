using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductUnitService : Repository<ProductUnits>, IProductUnitRepository
    {
        private readonly MasjidDBContext _context;

        public ProductUnitService(MasjidDBContext context) : base(context)
        {
            _context = context;
        }

        public Task<ProductUnits> AddAsync(ProductUnits entity)
        {
            throw new NotImplementedException();
        }

        public Task<ProductUnits> CreateAsync(ProductUnits productUnits)
        {
            throw new NotImplementedException();
        }

                
        public async Task<IEnumerable<ProductUnits>> GetAllUnitsAsync()
        {
            try
            {
                //IEnumerable<ProductUnits> units = new ();
                var units = await _context.ProductUnits.ToListAsync();
                return units;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
        public Task<Brand> GetUnitsById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ProductUnits entity)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ProductUnits>> IRepository<ProductUnits>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<ProductUnits?> IRepository<ProductUnits>.GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        Task<ProductUnits> IProductUnitRepository.GetUnitsById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
