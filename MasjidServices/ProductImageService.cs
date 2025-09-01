using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductImageService : Repository<ProductImage>, IProductImageRepository
    {
        private readonly MasjidDBContext _context;

        public ProductImageService(MasjidDBContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<ProductImageDTO>> GetProductImagesAsync(string productId)
        {

            try
            {
                var images = _context.ProductImages
                    .Where(pi => pi.ProductId == productId)
                    .Select(pi => new ProductImageDTO
                    {
                        ProductId = pi.ProductId,
                        ImageUrl = pi.ImageUrl,
                        BranchId = pi.BranchId,
                        CompanyId = pi.CompanyId


                    }).ToList();
                return Task.FromResult((IEnumerable<ProductImageDTO>)images);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return Task.FromResult(Enumerable.Empty<ProductImageDTO>());
            }


        }

        public Task<string> SaveProductImagesAsync(IEnumerable<ProductImageDTO> productImage)
        {
            try { 

                foreach (var img in productImage)
                {
                    var productImg = new ProductImage
                    {
                        ProductId = img.ProductId,
                        ImageUrl = img.ImageUrl,
                        BranchId = img.BranchId,
                        CompanyId = img.CompanyId
                    };
                    _context.ProductImages.Add(productImg);
                }
                _context.SaveChanges();
                return Task.FromResult("Success");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return Task.FromResult("Failed: " + ex.Message);
            }

        }
    }
}
