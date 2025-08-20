using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class Logging
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(100)]
        public required string UserId { get; set; }

        [MaxLength(250)]
        public required string SourceName { get; set; }

        public required string LogDescription { get; set; }

        public required DateTime EntryDateTime { get; set; }
    }
}
