using GravyFoodsApi.Models.Accounting;


namespace GravyFoodsApi.Models.Accounting
{
    public class JournalInfo : BaseEntity
    {
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public string SourceModule { get; set; }
        public string Description { get; set; }

        public bool IsPosted { get; set; }
        public string PostedBy { get; set; }
        public DateTime? PostedAt { get; set; }

        public List<JournalDetails> Lines { get; set; }
    }
}
