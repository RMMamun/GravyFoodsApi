using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class CustomerInfo
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Key]
        [StringLength(50)]
        public required string CustomerId { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string PhoneNo { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }


    public class CustomerInfoDTO
    {

        [Required]
        [StringLength(250)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string PhoneNo { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Email { get; set; } = string.Empty;
    }

}
