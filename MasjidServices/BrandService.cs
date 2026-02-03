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

        public BrandService(MasjidDBContext context, ITenantContextRepository tenant) : base(context)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<ApiResponse<IEnumerable<BrandDto>>> GetBrandsAsync()
        {
            ApiResponse<IEnumerable<BrandDto>> apiRes = new();

            try
            {
                //var brand = await _context.Brand.ToListAsync();

                IEnumerable<Brand> brands = await _context.Brand.Where(b => b.BranchId == _tenant.BranchId && b.CompanyId == _tenant.CompanyId).ToListAsync();
                IEnumerable<BrandDto> branchesDto = brands.Select(p => new BrandDto
                {

                    Id = p.Id,
                    Name = p.Name,
                    CountryOfOrigin = p.CountryOfOrigin,
                    IsActive = p.IsActive,
                    LogoUrl = p.LogoUrl


                });

                apiRes.Success = true;
                apiRes.Message = "Brands retrieved successfully.";
                apiRes.Data = branchesDto;

                return apiRes;

            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error retrieving brands.";
                apiRes.Errors = new List<string> { ex.Message };
                return apiRes;
            }
        }

        public async Task<ApiResponse<BrandDto>> GetBrandById(int Id)
        {
            ApiResponse<BrandDto> apiRes = new();

            try
            {
                var brand = await BrandExistsById(Id);
                if (brand == null)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Brand not found.";

                    return apiRes;
                }

                var result = new BrandDto
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    CountryOfOrigin = brand.CountryOfOrigin

                };

                apiRes.Success = true;
                apiRes.Message = "Brand retrieved successfully.";
                apiRes.Data = result;

                return apiRes;

            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error retrieving brand.";
                apiRes.Errors = new List<string> { ex.Message };

                return apiRes;
            }
        }

        public async Task<ApiResponse<BrandDto>> CreateAsync(BrandDto brand)
        {
            ApiResponse<BrandDto> apiRes = new();
            string _branchId = _tenant.BranchId;
            string _companyId = _tenant.CompanyId;

            try
            {
                var dto = await this.BrandExistsByName(brand.Name);
                if (dto != null )
                {
                    apiRes.Success = false;
                    apiRes.Message = "Duplicate brand name found!";

                    return apiRes;
                }

                var newBrand = new Brand
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    CountryOfOrigin = brand.CountryOfOrigin,
                    BranchId = _branchId,
                    CompanyId = _companyId,
                    IsActive = brand.IsActive,
                    LogoUrl = brand.LogoUrl,
                    

                };

                await _context.Brand.AddAsync(newBrand);
                await _context.SaveChangesAsync();


                var dto1  = await this.BrandExistsByName(brand.Name);
                if (dto1 != null)
                {
                    brand.Id = dto1.Id;
                    apiRes.Success = true;
                    apiRes.Message = "Brand created successfully.";
                    apiRes.Data = brand;
                }

                return apiRes;

            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error creating brand.";
                apiRes.Errors = new List<string> { ex.Message };

                return apiRes;
            }
        }

        


        public async Task<ApiResponse<bool>> UpdateAsync(BrandDto brand)
        {
            ApiResponse<bool> apiRes = new();
            try
            {
                var existingBrand = await _context.Brand.Where(b => b.Id == brand.Id & b.BranchId == _tenant.BranchId && b.CompanyId == _tenant.CompanyId).FirstOrDefaultAsync();
                if (existingBrand == null)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Brand not found.";
                    return apiRes;
                }
                existingBrand.Name = brand.Name;
                existingBrand.CountryOfOrigin = brand.CountryOfOrigin;
                existingBrand.IsActive = brand.IsActive;

                _context.Brand.Update(existingBrand);
                await _context.SaveChangesAsync();

                apiRes.Success = true;
                apiRes.Message = "Brand updated successfully.";
                apiRes.Data = true;
                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error updating brand.";
                apiRes.Errors = new List<string> { ex.Message };
                return apiRes;
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int Id)
        {
            ApiResponse<bool> apiRes = new();
            string _branchId = _tenant.BranchId;
            string _companyId = _tenant.CompanyId;


            try
            {
                var existingBrand = await BrandExistsById(Id);
                if (existingBrand == null)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Brand not found.";
                    return apiRes;
                }

                var brandEntity = new Brand
                {
                    Id = existingBrand.Id,
                    Name = existingBrand.Name,
                    CountryOfOrigin = existingBrand.CountryOfOrigin,
                    IsActive = existingBrand.IsActive,
                    LogoUrl = existingBrand.LogoUrl,
                    BranchId = _branchId,
                    CompanyId = _companyId,
                    
                };



                _context.Brand.Remove(brandEntity);
                await _context.SaveChangesAsync();
                apiRes.Success = true;
                apiRes.Message = "Brand deleted successfully.";
                apiRes.Data = true;
                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Error deleting brand.";
                apiRes.Errors = new List<string> { ex.Message };
                return apiRes;
            }
        }


        private async Task<BrandDto?> BrandExistsByName(string name)
        {
            string _branchId = _tenant.BranchId;
            string _companyId = _tenant.CompanyId;


            try
            {
                var brand = await _context.Brand.Where(b => b.Name.ToLower() == name.ToLower() && b.CompanyId == _tenant.CompanyId).FirstOrDefaultAsync();
                if (brand != null)
                {
                    var brandDto = new BrandDto
                    {
                        Id = brand.Id,
                        Name = brand.Name,
                        CountryOfOrigin = brand.CountryOfOrigin,
                        IsActive = brand.IsActive,
                        LogoUrl = brand.LogoUrl,
                        
                    };
                    return brandDto;
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<BrandDto?> BrandExistsById(int ID)
        {
            try
            {
                var brand = await _context.Brand.Where(b => b.Id == ID && b.CompanyId == _tenant.CompanyId).FirstOrDefaultAsync();
                if (brand != null)
                {
                    var brandDto = new BrandDto
                    {
                        Id = brand.Id,
                        Name = brand.Name,
                        CountryOfOrigin = brand.CountryOfOrigin,
                        IsActive = brand.IsActive,
                        LogoUrl = brand.LogoUrl
                    };
                    return brandDto;
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
