using GravyFoodsApi.Common;
using GravyFoodsApi.Data;
using GravyFoodsApi.DTO;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Repositories;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace GravyFoodsApi.MasjidServices
{
    public class ProductImageService : Repository<ProductImage>, IProductImageRepository
    {
        private readonly MasjidDBContext _context;

        private string imageDirectory = Path.Combine(Environment.CurrentDirectory, "ProductImages");


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
                string productId = productImage.Select(p => p.ProductId).FirstOrDefault() ?? string.Empty;


                //Delete existing images for the product from the Directory & Database
                this.DeleteProductImages(productId);
                

                foreach (var img in productImage)
                {

                    // Save image to directory and get the file path
                    string imagefilepath = await SaveImageToDirectory(img.ImageAsByte, img.ImageName);
                    imagefilepath = $"{GlobalVariable.StaticFileDir}/{img.ImageName}"  ; // For URL purpose

                    var productImg = new ProductImage
                    {
                        ProductId = img.ProductId,
                        ImageUrl = imagefilepath,
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



        public async Task<string> SaveImageToDirectory(byte[] imageData , string fileName)
        {
            try
            {
                //byte[] imageData = fileData.FileData;
                //string fileName = fileData.fileName;

                //string imageDirectory = @"C:\MasjidImages";
                

                // Create the directory if it doesn't exist
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                //string fileName = $"{fileName}.jpg";
                string filePath = Path.Combine(imageDirectory, fileName); // Set your desired upload directory
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

        public Task<bool> DeleteProductImages(string productid)
        {
            try
            {
                //Get all images of the product
                IList<ProductImage?> img = _context.ProductImages.Where(p => p.ProductId == productid).ToList();
                
                //Delete all images of the product
                _context.ProductImages.Where(p => p.ProductId == productid).ExecuteDelete();

                string fileDirectory = Path.Combine(Environment.CurrentDirectory, "MasjidImages");

                foreach (var item in img)
                {
                    if (item != null)
                    {
                        string filePath = Path.Combine(fileDirectory, item.ImageUrl);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }

                _context.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return Task.FromResult(false);
            }
        }


    }
}
