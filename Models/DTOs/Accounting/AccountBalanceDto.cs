using GravyFoodsApi.Models.Accounting;

namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class AccountBalanceDto
    {
        public Guid AccountId { get; set; }


        public string AccountCode { get; set; }
        public string AccountName { get; set; }

        public AccountType AccountType { get; set; }
        //public AccountCategory AccountCategory { get; set; }

        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }

        public decimal Balance { get; set; }

        public DateTime AsOfDate { get; set; }
    }
}
