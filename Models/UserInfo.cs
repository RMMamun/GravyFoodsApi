//using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class UserInfo
    {
        //[System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Key]
        [Required]
        [MaxLength(50)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DeviceId { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(300)]
        public string UserName { get; set; }

        [MaxLength(300)]
        public string? UserRole { get; set; }

        [MaxLength(100)]
        public string? MasjidID { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime EntryDateTime { get; set; }

        public string? Email { get; set; }

        public string? BranchId { get; set; }
        public string? CompanyId { get; set; }

    }

    public class UserInfoDTO
    {

        [Required]
        [MaxLength(50)]
        public string DeviceId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserId { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(300)]
        public string UserName { get; set; }

        [MaxLength(300)]
        public string? UserRole { get; set; }

        [MaxLength(100)]
        public string? MasjidID { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string? Email { get; set; }

        public string? BranchId { get; set; }
        public string? CompanyId { get; set; }

        ////[System.ComponentModel.DataAnnotations.Key]
        ////public long Id { get; set; }
        //public string UserId { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }

    }


    public class UserBasicInfoDTO
    {

        [Required]
        [MaxLength(50)]
        public string UserId { get; set; }

        [MaxLength(300)]
        public string UserName { get; set; }

        [MaxLength(300)]
        public string? UserRole { get; set; }

        public DateTime EntryDateTime { get; set; }
        public string? Email { get; set; }

        public string? BranchId { get; set; }
        public string? CompanyId { get; set; }

    }


}
