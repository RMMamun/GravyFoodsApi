namespace GravyFoodsApi.MasjidRepository
{
    
    public interface ITenantContextRepository
    {
        string CompanyId { get; }
        string BranchId { get; }
        string UserId { get; }
    }


}
