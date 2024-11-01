using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MasjidApi.Models
{
    public class EventTypes
    {
        [JsonPropertyName("id")]
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [JsonPropertyName("eventType")]
        [MaxLength(150)]
        public required string EventType { get; set; }
    }
}
