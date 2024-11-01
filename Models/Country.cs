using Microsoft.EntityFrameworkCore;

namespace MasjidApi.Models
{
    [Keyless]
    public class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public string FlagImagePath { get; set; }
    }

    public class CountryView
    {
        public Int64 Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public string FlagImagePath { get; set; }

        public ICollection<MasjidInfo> MasjidInfos { get; set; }
    }
}
