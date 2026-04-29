namespace GravyFoodsApi.Models.DTOs.Accounting
{
    public class LedgerDto
    {
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }   // Move (Voucher)
        public string Description { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public decimal Balance { get; set; }

        public string SourceModule { get; set; } // POS / Purchase / Manual
        public bool IsPosted { get; set; }
    }


}
