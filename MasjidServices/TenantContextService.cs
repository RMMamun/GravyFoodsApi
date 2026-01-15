using GravyFoodsApi.MasjidRepository;
using System.Security.Claims;

namespace GravyFoodsApi.MasjidServices
{
    
    public class TenantContextService : ITenantContextRepository
    {
        public string CompanyId { get; }
        public string BranchId { get; }

        public TenantContextService(IHttpContextAccessor accessor)
        {
            var user = accessor.HttpContext!.User;
            CompanyId = (user.FindFirstValue("companyId"));
            BranchId = (user.FindFirstValue("branchId"));
        }
    }


}
