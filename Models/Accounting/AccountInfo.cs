namespace GravyFoodsApi.Models.Accounting
{
    public class AccountInfo : BaseEntity
    {
        public string ACCode { get; set; } 
        public string ACName { get; set; }
        public AccountType AccountType { get; set; }

        public Guid? ParentId { get; set; }
        public bool IsControlAccount { get; set; }
        public bool IsActive { get; set; }
    }
}
