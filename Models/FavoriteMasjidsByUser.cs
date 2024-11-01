using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasjidApi.Models
{
    public class FavoriteMasjidsByUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string UserId { get; set; }
        public string MasjidID { get; set; }
        public int Sequence { get; set; }
    }
}
