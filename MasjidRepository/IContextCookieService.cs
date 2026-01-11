namespace GravyFoodsApi.MasjidRepository
{
    public interface IContextCookieService
    {
        //void SetBranchContext(HttpResponse response, Guid tenantId, Guid branchId);
        void SetBranchContext(HttpResponse response, string tenantId, string branchId);
    }

}
