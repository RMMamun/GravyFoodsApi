using GravyFoodsApi.Models.Accounting;

namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class AccountInfoDto
    {
        public Guid Id { get; set; }

        //[Required(ErrorMessage = "Account Code is required")]
        public string ACCode { get; set; } = "";

        //[Required(ErrorMessage = "Account Name is required")]
        public string ACName { get; set; } = "";
        public required AccountType ACType { get; set; }
        public string? Description { get; set; }

        public Guid? ParentId { get; set; }
        public string? ParentACCode { get; set; }
        public string? ParentName { get; set; }
        public bool IsControlAccount { get; set; }
        public bool IsActive { get; set; }

        public List<AccountInfoDto>? Children { get; set; }
    }


}
