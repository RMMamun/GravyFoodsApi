namespace GravyFoodsApi.MasjidRepository
{
    public interface IContextCookieService
    {
        void SetBranchContext(HttpResponse response, Guid tenantId, Guid branchId);
    }

}
