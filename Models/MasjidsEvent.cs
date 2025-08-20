using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;



namespace GravyFoodsApi.Models
{
    public class MasjidsEvent
    {
        [JsonPropertyName("eventId")]
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EventId { get; set; }

        /* MasjidId is a foreign key*/
        [JsonPropertyName("masjidID")]
        [MaxLength(100)]
        public required string MasjidID { get; set; }

        [JsonPropertyName("eventTitle")]
        [MaxLength(150)]
        public required string EventTitle { get; set; }

        [JsonPropertyName("eventDescription")]
        [MaxLength(500)]
        public required string EventDescription { get; set; }

        [JsonPropertyName("eventStartDate")]
        public DateTime EventStartDate { get; set; }

        [JsonPropertyName("eventEndDate")]
        public DateTime EventEndDate { get; set; }

        [JsonPropertyName("entryDateTime")]
        public DateTime EntryDateTime { get; set; }

    }
}
