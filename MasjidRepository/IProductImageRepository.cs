using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using System;

namespace GravyFoodsApi.MasjidRepository
{
    public interface IProductImageRepository : IRepository<ProductImage> 
    {
        Task<string> SaveProductImagesAsync(IEnumerable<ProductImageDTO> productImage);
        Task<IEnumerable<ProductImageDTO>> GetProductImagesAsync(string productId);

        Task<IEnumerable<ProductImageDTO>> GetAllProductImagesAsync(AllProductImageGetParameterDto allProductImages);

    }

    //public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    //{
    //    public ProductImageRepository(MasjidDBContext context) : base(context) { }
    //}
}
