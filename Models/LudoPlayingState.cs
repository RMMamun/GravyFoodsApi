
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MasjidApi.Models
{
    public class LudoPlayingState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public required string SessionId { get; set; }
        public required string PlayerId { get; set; }
        public required string MappingId { get; set; }
        public required string isActive { get; set; }

        public required string isPlayerActive { get; set; }
        public required string MyTurn { get; set; }
        public required int DiceValue { get; set; }
        public required int SelectedValue { get; set; }
        public required int SelectedBall { get; set; }
        public required string wasRead { get; set; }

        //public required int Ball1 { get; set; }
        //public required int Ball2 { get; set; }
        //public required int Ball3 { get; set; }
        //public required int Ball4 { get; set; }
    }
}
