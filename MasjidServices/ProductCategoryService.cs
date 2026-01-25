using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductCategoryService : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public ProductCategoryService(MasjidDBContext context, ITenantContextRepository tenant) : base(context)
        {
            _context = context;
            _tenant = tenant;
        }

        public Task<ProductCategory> CreateCategoryAsync(ProductCategory productCategory)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
        {
            try
            {

                return await _context.ProductCategory.Where(w => w.BranchId == _tenant.BranchId && w.CompanyId == _tenant.CompanyId).ToListAsync();

            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }

        }


        public Task<ProductCategory> GetCategoryById(int Id)
        {
            throw new NotImplementedException();
        }

    }
}
