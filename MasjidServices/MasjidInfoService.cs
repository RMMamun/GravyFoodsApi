using MasjidApi.Common;
using MasjidApi.Data;
using MasjidApi.DTO;
using MasjidApi.MasjidRepository;
using MasjidApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MasjidApi.MasjidServices
{
    public class MasjidInfoService : IMasjidInfoService
    {
        GeoLocationService geoLoc = new GeoLocationService();
        CommonMethods commonMethods = new CommonMethods();
        private readonly MasjidDBContext _dbContext;
        public MasjidInfoService(MasjidDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MasjidInfo>> GetAllMasjids(double latitude, double longitude, string address, int kilometers = 1)
        {
            try
            {
                ////MasjidInfo newMasjid = new MasjidInfo();
                ////var result = _dbContext.MasjidInfo.Where(x => x.Latitude == latitude).ToListAsync();

                ////var newMasjid = _dbContext.MasjidInfo
                ////.OrderBy(c => commonMethods.CalculateHaversineDistance(latitude, longitude, c.Latitude.GetValueOrDefault(), c.Longitude.GetValueOrDefault()))
                ////.FirstOrDefault();

                ////var qry = _dbContext.MasjidInfo.Where(x => x.Latitude >  latitude - 2 && x.Latitude < latitude + 2);

                ////var qry = await _dbContext.MasjidInfo.Where(x => x.Latitude > latitude - 2 && x.Latitude < latitude + 2).ToListAsync();

                //var qry = await _dbContext.MasjidInfo.Where(x => (x.Latitude >= latitude && x.Latitude <= tolatitude) && (x.Longitude >= longitude && x.Longitude <= tolongitude)).ToListAsync();

                ////var newMasjid = await _dbContext.MasjidInfo.ToListAsync();

                ////var sqlqry = qry.ToQueryString();
                ////var newMasjid = qry.ToListAsync();

                //Chat-GPT -> Haversine formula.
                // Define the latitude and longitude of the center point
                double centerLatitude = latitude;
                double centerLongitude = longitude;

                // Define the maximum distance in kilometers
                if (kilometers <= 0)
                {
                    kilometers = 1;
                }

                double maxDistanceKm = kilometers;

                // Calculate the minimum and maximum latitude and longitude values based on the center point and maximum distance
                double minLatitude = centerLatitude - (maxDistanceKm / 111.12);
                double maxLatitude = centerLatitude + (maxDistanceKm / 111.12);
                double minLongitude = centerLongitude - (maxDistanceKm / (111.12 * Math.Cos(centerLatitude * Math.PI / 180)));
                double maxLongitude = centerLongitude + (maxDistanceKm / (111.12 * Math.Cos(centerLatitude * Math.PI / 180)));

                // Query the database using the calculated latitude and longitude ranges
                var qry = await _dbContext.MasjidInfo
                    .Where(x => x.Latitude >= minLatitude && x.Latitude <= maxLatitude && x.Longitude >= minLongitude && x.Longitude <= maxLongitude
                    && ((!string.IsNullOrEmpty(address) ? x.Address.Contains(address) : true) || (!string.IsNullOrEmpty(address) ? x.MasjidName.Contains(address) : true)))
                    .ToListAsync();

                return qry;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<MasjidInfoDTO> Create(MasjidInfoDTO masjidInfoDTO)
        {
            try
            {
                bool isNew = false;
                var masjid = await _dbContext.MasjidInfo.Where(x => x.MasjidID == masjidInfoDTO.MasjidID).FirstOrDefaultAsync();
                
                if (masjid != null)     //UPDATE
                {
                    masjid.MasjidName = masjidInfoDTO.MasjidName;
                    masjid.Address = masjidInfoDTO.Address;
                    masjid.Latitude = masjidInfoDTO.Latitude;
                    masjid.Longitude = masjidInfoDTO.Longitude;
                    masjid.Capacity = masjidInfoDTO.Capacity;
                    masjid.City = masjidInfoDTO.City;
                    masjid.ContactNumber = masjidInfoDTO.ContactNumber;
                    masjid.CountryId = masjidInfoDTO.CountryId == 0 ? 1 : masjidInfoDTO.CountryId;
                    masjid.Email = masjidInfoDTO.Email;
                    masjid.Website = masjidInfoDTO.Website;
                    masjid.ImagePath = masjidInfoDTO.ImagePath;
                    masjid.ImageAsByte = masjidInfoDTO.ImageAsByte;
                    masjid.EntryDate = masjid.EntryDate;
                    masjid.Description = masjidInfoDTO.Description;
                    masjid.IsWaterAvailable = masjidInfoDTO.IsWaterAvailable;
                    masjid.IsWomanPlaceAvailable = masjidInfoDTO.IsWomanPlaceAvailable;


                    await _dbContext.SaveChangesAsync();

                    return MasjidDTO(masjid);
                }
                else        //CREATE
                {
                    isNew = true;

                    var newMasjid = new MasjidInfo
                    {
                        //"MasjidID,MasjidName,Address,Latitude,Longitude,ContactNumber,City,CountryId,UserId,Email,Website,ImagePath"
                        MasjidID = masjidInfoDTO.MasjidID,
                        MasjidName = masjidInfoDTO.MasjidName,
                        Address = masjidInfoDTO.Address,
                        Latitude = masjidInfoDTO.Latitude,
                        Longitude = masjidInfoDTO.Longitude,
                        Capacity = masjidInfoDTO.Capacity,
                        City = masjidInfoDTO.City,
                        ContactNumber = masjidInfoDTO.ContactNumber,
                        CountryId = masjidInfoDTO.CountryId == 0 ? 1 : masjidInfoDTO.CountryId,
                        Email = masjidInfoDTO.Email,
                        Website = masjidInfoDTO.Website,
                        ImagePath = masjidInfoDTO.ImagePath,
                        EntryDate = masjidInfoDTO.EntryDate,
                        Description = masjidInfoDTO.Description,
                        IsWaterAvailable = masjidInfoDTO.IsWaterAvailable,
                        IsWomanPlaceAvailable = masjidInfoDTO.IsWomanPlaceAvailable


                    };

                    var result = await _dbContext.MasjidInfo.AddAsync(newMasjid);
                    await _dbContext.SaveChangesAsync();

                    //await SaveImage(masjidInfoDTO.ImageByteData);

                    if (isNew == true)
                    {
                        var isCopied = await CopyAndSavePrayerTimeOfNearestMasjid((double)newMasjid.Latitude, (double)newMasjid.Longitude);

                    }

                    return MasjidDTO(newMasjid);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<bool> CopyAndSavePrayerTimeOfNearestMasjid(double latitude, double longitude)
        {
            try
            {
                string address = "";
                int kilo = 1;

                var neasrest = await GetAllMasjids(latitude, longitude, address, kilo);

                if (neasrest != null)
                {
                    foreach (var item in neasrest)
                    {
                        double distance = geoLoc.CalculateDistance(latitude, longitude, (double)item.Longitude, (double)item.Longitude);

                    }


                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> SaveImage(FileTransferModel fileData)
        {
            try
            {
                byte[] imageData = fileData.FileData;
                string fileName = fileData.fileName;

                //string uploadDirectory = @"C:\MasjidImages";
                string uploadDirectory = Path.Combine(Environment.CurrentDirectory, "MasjidImages");

                // Create the directory if it doesn't exist
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                //string fileName = $"{fileName}.jpg";
                string filePath = Path.Combine(uploadDirectory, fileName); // Set your desired upload directory
                await System.IO.File.WriteAllBytesAsync(filePath, imageData);

                return true;
            }
            catch (Exception ex)
            {
                return false;
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


        public Task<bool> Delete(MasjidInfoDTO masjidDto)
        {
            throw new NotImplementedException();
        }

        public async Task<MasjidInfoDTO> GetMasjidInfoById(string id)
        {
            try
            {
                var masjid = await _dbContext.MasjidInfo.Where(x => x.MasjidID == id).FirstOrDefaultAsync();
                if (masjid == null)
                {
                    return null;
                }
                else
                {
                    if (masjid.ImagePath != "")
                    {
                        byte[] masjidImage = await GetImageDataFromDevice(masjid.ImagePath);
                        masjid.ImageAsByte = masjidImage;
                    }

                    return MasjidDTO(masjid);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> IsExistes(string masjidname)
        {
            try
            {
                bool existed = false;
                var masjid = await _dbContext.MasjidInfo.FindAsync(masjidname);
                if (masjid != null)
                {
                    existed = true;
                }
                return existed;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<MasjidInfoDTO> Update(MasjidInfoDTO masjidDto)
        {
            try
            {
                //if (id != masjidDto.MasjidID)
                //{
                //    return null;
                //}

                var masjid = await _dbContext.MasjidInfo.FindAsync(masjidDto.MasjidID);
                if (masjid == null)
                {
                    return null;
                }

                masjid.MasjidID = masjidDto.MasjidID;
                masjid.MasjidName = masjidDto.MasjidName;
                masjid.Address = masjidDto.Address;
                masjid.Latitude = masjidDto.Latitude;
                masjid.Longitude = masjidDto.Longitude;
                masjid.City = masjidDto.City;
                masjid.ContactNumber = masjidDto.ContactNumber;
                masjid.CountryId = 1;
                masjid.Email = masjidDto.Email;
                masjid.Website = masjidDto.Website;
                masjid.ImagePath = masjidDto.ImagePath;
                masjid.EntryDate = masjidDto.EntryDate;
                masjid.Description = masjidDto.Description;
                masjid.IsWaterAvailable = masjidDto.IsWaterAvailable;
                masjid.IsWomanPlaceAvailable = masjidDto.IsWomanPlaceAvailable;

                try
                {
                    await _dbContext.SaveChangesAsync();
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<string> UpdateFacilitiesAsync(MasjidFacilityDTO dataDto)
        {
            try
            {
                var masjidToUpdate = await _dbContext.MasjidInfo.FindAsync(dataDto.MasjidID);
                if (masjidToUpdate != null)
                {
                    masjidToUpdate.IsWaterAvailable = dataDto.IsWaterAvailable;
                    masjidToUpdate.IsWomanPlaceAvailable = dataDto.IsWomanPlaceAvailable;

                    await _dbContext.SaveChangesAsync();

                    return "OK";
                }
                else
                {
                    return "NotFound";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static MasjidInfoDTO MasjidDTO(MasjidInfo masjidInfo) =>
        new MasjidInfoDTO
        {
            //"MasjidID,MasjidName,Address,Latitude,Longitude,ContactNumber,City,CountryId,UserId,Email,Website,ImagePath"

            MasjidID = masjidInfo.MasjidID,
            MasjidName = masjidInfo.MasjidName,
            Address = masjidInfo.Address,
            Latitude = masjidInfo.Latitude,
            Longitude = masjidInfo.Longitude,
            ContactNumber = masjidInfo.ContactNumber,
            Email = masjidInfo.Email,
            Website = masjidInfo.Website,
            City = masjidInfo.City,
            CountryId = masjidInfo.CountryId,
            ImagePath = masjidInfo.ImagePath,
            Capacity = masjidInfo.Capacity,
            Description = masjidInfo.Description,
            EntryDate = masjidInfo.EntryDate,
            IsWaterAvailable = masjidInfo.IsWaterAvailable,
            IsWomanPlaceAvailable = masjidInfo.IsWomanPlaceAvailable,
            ImageAsByte = masjidInfo?.ImageAsByte,


        };
    }
}