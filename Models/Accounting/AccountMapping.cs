namespace GravyFoodsApi.Models.Accounting
{
    public class AccountMapping
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }

        public string CashAccountId { get; set; }
        public string BankAccountId { get; set; }
        public string SalesAccountId { get; set; }
        public string VatAccountId { get; set; }
        public string CogsAccountId { get; set; }
        public string InventoryAccountId { get; set; }
        public string ReceivableAccountId { get; set; }
    }
}
