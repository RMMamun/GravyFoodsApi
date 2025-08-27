using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductCategoryService : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly MasjidDBContext _context;

        public ProductCategoryService(MasjidDBContext context) : base(context)
        {
            _context = context;
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

                return await _context.ProductCategories.ToListAsync();

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
