
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.DTO
{
    public class DiceTurnDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public required string SessionId { get; set; }
        public required string MappingId { get; set; }
        public required string MyTurn { get; set; }
    }
}
