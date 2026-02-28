using GravyFoodsApi.Models.Accounting;

namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class AccountInfoDto
    {
        public string Id { get; set; }
        public string ACCode { get; set; }
        public string ACName { get; set; }
        public AccountType ACType { get; set; }
        public string? Description { get; set; }

        public string? ParentId { get; set; }
        public string? ParentACCode { get; set; }
        public string? ParentName { get; set; }
        public bool IsControlAccount { get; set; }
        public bool IsActive { get; set; }
    }


}
