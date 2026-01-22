using System.ComponentModel.DataAnnotations;

namespace GravyFoodsApi.Models.DTOs
{

    public class CustomerInfoDTO
    {


        [StringLength(50)]
        public string CustomerId { get; set; } = string.Empty;

        [StringLength(250)]
        public string CustomerName { get; set; } = string.Empty;


        public string Address { get; set; } = string.Empty;


        [StringLength(50)]
        public string PhoneNo { get; set; } = string.Empty;


        [StringLength(250)]
        public string Email { get; set; } = string.Empty;

        public string BranchId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
    }

}
