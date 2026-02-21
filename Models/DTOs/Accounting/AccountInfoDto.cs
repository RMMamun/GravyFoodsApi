using GravyFoodsApi.Models.Accounting;

namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class AccountInfoDto
    {
        public string Id { get; set; }
        public string ACCode { get; set; }
        public string ACName { get; set; }
        public AccountType AccountType { get; set; }

        public Guid? ParentId { get; set; }
        public bool IsControlAccount { get; set; }
        public bool IsActive { get; set; }
    }
}
