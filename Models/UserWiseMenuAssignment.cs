using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models
{
    public class UserWiseMenuAssignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int MenuId { get; set; }

        [Required]
        public string BranchId { get; set; }

        [Required]
        public string CompanyId { get; set; }


        [ForeignKey(nameof(MenuId))]
        public NavMenuItem Menu { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserInfo UserInfo { get; set; }

    }

}
