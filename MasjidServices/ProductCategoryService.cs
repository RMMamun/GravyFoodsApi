using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductCategoryService : IProductCategoryRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public ProductCategoryService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<ApiResponse<bool>> CreateCategoryAsync(ProductCategoryDto productCategory)
        {
            ApiResponse<bool> apiRes = new();
            try
            {
                var newCategory = new ProductCategory
                {
                    Name = productCategory.Name,
                    Description = productCategory.Description,
                    IsActive = productCategory.IsActive,
                    SortOrder = productCategory.SortOrder,
                    ParentId = productCategory.ParentId,
                    BranchId = _tenant.BranchId,
                    CompanyId = _tenant.CompanyId
                };
                await _context.ProductCategory.AddAsync(newCategory);
                await _context.SaveChangesAsync();
                
                apiRes.Data = true;
                apiRes.Success = true;
                apiRes.Message = "Category created successfully";
                
                return apiRes;


            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;
                return apiRes;
            }
        }

        public async Task<ApiResponse<IEnumerable<ProductCategoryDto>>> GetAllCategoriesAsync()
        {
            ApiResponse<IEnumerable<ProductCategoryDto>> apiRes = new ();
            try
            {

                var result = await _context.ProductCategory.Where(w => w.BranchId == _tenant.BranchId && w.CompanyId == _tenant.CompanyId).ToListAsync();
                var mappedResult = result.Select(s => new ProductCategoryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    IsActive = s.IsActive,
                    SortOrder = s.SortOrder,
                    ParentId = s.ParentId
                });

                apiRes.Data = mappedResult;
                apiRes.Success = true;
                return apiRes;


            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;
                return apiRes;

            }

        }

        
        public async Task<ApiResponse<ProductCategoryDto>> GetCategoryById(int Id)
        {
            ApiResponse<ProductCategoryDto> apiRes = new();
            try
            {

                var result = await _context.ProductCategory.FirstOrDefaultAsync(w => w.Id == Id && w.BranchId == _tenant.BranchId && w.CompanyId == _tenant.CompanyId);
                if (result == null)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Category not found";
                    return apiRes;
                }

                var mappedResult = new ProductCategoryDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    IsActive = result.IsActive,
                    SortOrder = result.SortOrder,
                    ParentId = result.ParentId,
                };

                apiRes.Data = mappedResult;
                apiRes.Success = true;
                return apiRes;


            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;
                return apiRes;

            }
        }

    }
}
