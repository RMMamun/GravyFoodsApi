using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    

    //public class BrandService : Repository<Brand>, IBrandRepository
    public class BrandService : Repository<Brand>, IBrandRepository
    {
        private readonly MasjidDBContext _context;

        public BrandService(MasjidDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetBrandsAsync()
        {
            try
            {
                var brand = await _context.Brand.ToListAsync();

                return brand;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Brand> GetBrandById (int Id)
        {
            try
            {
                var brand = await _context.Brand.Where(b => b.Id == Id).FirstOrDefaultAsync();

                return brand;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<Brand> CreateAsync(Brand brand)
        {
            throw new NotImplementedException();
        }
    }
}
