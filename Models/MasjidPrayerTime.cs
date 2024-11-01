using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasjidApi.Models
{
    
    public class MasjidPrayerTime
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MPTId { get; set; }
        public string MasjidID { get; set; }
        public TimeSpan Fajr { get; set; }
        public TimeSpan Dhuhr { get; set; }
        public TimeSpan Asr { get; set; }
        public TimeSpan Maghrib { get; set; }
        public TimeSpan Isha { get; set; }
        public TimeSpan Jummah { get; set; }
        public TimeSpan? EidUlFitr { get; set; }
        public TimeSpan? EidUlAzha { get; set; }
    }
}
