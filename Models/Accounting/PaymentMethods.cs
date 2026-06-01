using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models.Accounting
{
    public class PaymentMethods
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodCode { get; set; }

        public Guid AccountId { get; set; }
        public int SequenceNo { get; set; }
        public bool SystemData { get; set; }
        public string CompanyId { get; set; }

    }
}
