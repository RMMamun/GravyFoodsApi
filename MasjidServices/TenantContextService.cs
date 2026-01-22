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
            try
            {
                var user = accessor.HttpContext!.User;
                var company = (user.FindFirstValue("companyId"));
                var branch = (user.FindFirstValue("branchId"));

                CompanyId = string.IsNullOrEmpty(company) == false ? company : "";
                BranchId = string.IsNullOrEmpty(branch) == false ? branch : "";
            }
            catch (Exception ex)
            {
                CompanyId = "";
                BranchId = "";
            }
        }
    }


}
