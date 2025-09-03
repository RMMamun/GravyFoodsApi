using GravyFoodsApi.Data;
using GravyFoodsApi.DTO;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<string> SaveProductImagesAsync(IEnumerable<ProductImageDTO> productImage)
        {
            try
            {
                var isSaved = false;
                foreach (var img in productImage)
                {
                    var productImg = new ProductImage
                    {
                        ProductId = img.ProductId,
                        ImageUrl = await this.SaveImage(img.ImageAsByte, img.ImageName),
                        BranchId = img.BranchId,
                        CompanyId = img.CompanyId,

                    };
                    _context.ProductImages.Add(productImg);

                    
                }
                _context.SaveChanges();
                return ("Success");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return ("Failed: " + ex.Message);
            }

        }



        public async Task<string> SaveImage(byte[] imageData , string fileName)
        {
            try
            {
                //byte[] imageData = fileData.FileData;
                //string fileName = fileData.fileName;

                //string uploadDirectory = @"C:\MasjidImages";
                string uploadDirectory = Path.Combine(Environment.CurrentDirectory, "ProductImages");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                //string fileName = $"{fileName}.jpg";
                string filePath = Path.Combine(uploadDirectory, fileName); // Set your desired upload directory
                await System.IO.File.WriteAllBytesAsync(filePath, imageData);

                return filePath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<byte[]> GetImageDataFromDevice(string imgFileName)
        {
            try
            {
                // Platform-specific code to get the image data
                // For example, using MediaPicker in Xamarin Essentials
                // Here's an example for Android:
                // var photo = MediaPicker.PickPhotoAsync();

                string fileDirectory = Path.Combine(Environment.CurrentDirectory, "MasjidImages");

                // For demo purposes, you can use a placeholder image
                var _image = System.IO.File.ReadAllBytes(fileDirectory + "/" + imgFileName);
                return _image;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
