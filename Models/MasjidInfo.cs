using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MasjidApi.Models
{
    //[Keyless]
    public class MasjidInfo
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string MasjidID { get; set; }
		public string MasjidName { get; set; }
		public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? ContactNumber { get; set; }
        public string? City { get; set; }
        public Int64 CountryId { get; set; }

        public int? Capacity { get; set; } = 0;
        public string? Description { get; set; }
        public DateTime EntryDate { get; set; }

        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? ImagePath { get; set; }
        public byte[]? ImageAsByte { get; set; }
        public int? IsWaterAvailable { get; set; } = 0;
        public int? IsWomanPlaceAvailable { get; set; } = 0;



    }

    public class MasjidInfoDTO
    {
        public string MasjidID { get; set; }
        public string MasjidName { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? ContactNumber { get; set; }
        public string? City { get; set; }
        public Int64 CountryId { get; set; }

        public int? Capacity { get; set; } = 0;
        public string? Description { get; set; }
        public DateTime EntryDate { get; set; }

        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? ImagePath { get; set; }
        //public byte[]? ImageByteData { get; set; }
        public byte[]? ImageAsByte { get; set; }
        public int? IsWaterAvailable { get; set; } = 0;
        public int? IsWomanPlaceAvailable { get; set; } = 0;
    }


    public class MasjidFacilityDTO
    {
        public string MasjidID { get; set; }
        
        public int? IsWaterAvailable { get; set; } = 0;
        public int? IsWomanPlaceAvailable { get; set; } = 0;
    }


}
