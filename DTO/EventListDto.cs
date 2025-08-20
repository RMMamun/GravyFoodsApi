using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GravyFoodsApi.DTO
{
    public class EventListDto
    {

        [JsonPropertyName("eventId")]
        public long EventId { get; set; }

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

        [JsonPropertyName("userId")]
        public string UserId { get; set; }


        [JsonPropertyName("masjidName")]
        public string MasjidName { get; set; }
    }
}
