using MasjidApi.DTO;
using MasjidApi.Models;

namespace MasjidApi.MasjidRepository
{
    public interface IMasjidInfoService
    {
        Task<MasjidInfoDTO> Create(MasjidInfoDTO masjidDto);
        Task<MasjidInfoDTO> GetMasjidInfoById(string id);
        Task<MasjidInfoDTO> Update(MasjidInfoDTO masjidDto);

        Task<string> UpdateFacilitiesAsync(MasjidFacilityDTO dataDto);

        Task<bool> Delete(MasjidInfoDTO masjidDto);
        Task<bool> IsExistes(string masjidname);

        Task<IEnumerable<MasjidInfo>> GetAllMasjids(double latitude, double longitude, string address, int miles = 1);

        Task<bool> SaveImage(FileTransferModel fileData);

        Task<byte[]> GetImageDataFromDevice(string imgFileName);



    }
}
