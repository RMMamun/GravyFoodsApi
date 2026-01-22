using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace GravyFoodsApi.MasjidServices
{
    

    //public class BrandService : Repository<BrandDto>, IBrandRepository
    public class BrandService : Repository<Brand>, IBrandRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public BrandService(MasjidDBContext context,ITenantContextRepository tenant) : base(context)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
        {
            try
            {
                //var brand = await _context.Brand.ToListAsync();

                IEnumerable<Brand> brands = await _context.Brand.Where(b => b.BranchId == _tenant.BranchId && b.CompanyId == _tenant.CompanyId).ToListAsync();
                IEnumerable<BrandDto> branchesDto = brands.Select(p => new BrandDto
                {

                    Id = p.Id,
                    Name = p.Name,
                    CountryOfOrigin = p.CountryOfOrigin,


                });

                return branchesDto;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BrandDto> GetBrandById (int Id)
        {
            try
            {
                var brand = await _context.Brand.Where(b => b.Id == Id & b.BranchId == _tenant.BranchId && b.CompanyId == _tenant.CompanyId).FirstOrDefaultAsync();
                if (brand == null)
                {
                    return new BrandDto();
                }

                return new BrandDto
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    CountryOfOrigin = brand.CountryOfOrigin

                };

            }
            catch (Exception ex)
            {
                return new BrandDto();
            }
        }

        public Task<BrandDto> CreateAsync(BrandDto brand)
        {
            throw new NotImplementedException();
        }
    }
}
