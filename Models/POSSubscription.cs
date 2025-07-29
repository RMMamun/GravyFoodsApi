using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasjidApi.Models
{
    public class POSSubscription
    {
        //[System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Key]
        [MaxLength(250)]
        public required string DeviceKey { get; set; }

        [MaxLength(50)]
        public required string ClientKey { get; set; }

        [MaxLength(300)]
        public required string ClientName { get; set; }

        [MaxLength(300)]
        public required string ClientAddress { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }

        public required DateTime EntryDateTime { get; set; }
        public required DateTime SubscriptionStartDate { get; set; }
        public required DateTime SubscriptionEndDate { get; set; }


    }


}
