
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MasjidApi.Models
{
    public class LudoSession
    {
        
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public required string SessionId { get; set; }
        public required string PlayerId { get; set; }
        public required string PlayerName { get; set; }
        public int WorldRank { get; set; } = 0;
        public int Points { get; set; } = 0;
        public required string MappingId { get; set; }
        public required string isActive { get; set; }
        public byte[]? PlayerImageAsByte { get; set; }
        public int Sequence { get; set; }
        public DateTime GameDateTime { get; set; }
    }
}
