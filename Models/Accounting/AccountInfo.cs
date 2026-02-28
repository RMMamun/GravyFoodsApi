namespace GravyFoodsApi.Models.Accounting
{
    public class AccountInfo : BaseEntity
    {
        public string ACCode { get; set; } 
        public string ACName { get; set; }
        public AccountType ACType { get; set; }
        public string? Description { get; set; }

        
        public bool IsControlAccount { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? ParentACCode { get; set; }
        // ⭐ navigation property (self reference)
        public AccountInfo? Parent { get; set; }

        // ⭐ optional but recommended (children collection)
        public ICollection<AccountInfo>? Children { get; set; }

    }
}
