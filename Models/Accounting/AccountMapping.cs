namespace GravyFoodsApi.Models.Accounting
{
    public class AccountMapping
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }

        public Guid CashAccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid SalesAccountId { get; set; }
        public Guid VatAccountId { get; set; }
        public Guid CogsAccountId { get; set; }
        public Guid InventoryAccountId { get; set; }
        public Guid ReceivableAccountId { get; set; }
    }
}
