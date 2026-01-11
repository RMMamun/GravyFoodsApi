using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [ApiController]
    public abstract class PosBaseController : ControllerBase
    {
        protected string TenantId =>
            HttpContext.Items["TenantId"] is string t
                ? t
                : throw new UnauthorizedAccessException("Tenant context missing");

        protected Guid BranchId =>
            HttpContext.Items["BranchId"] is Guid b
                ? b
                : throw new UnauthorizedAccessException("Branch context missing");
    }

}
