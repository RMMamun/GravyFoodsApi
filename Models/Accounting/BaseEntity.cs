namespace GravyFoodsApi.Models.Accounting
{
 
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
    }
}
