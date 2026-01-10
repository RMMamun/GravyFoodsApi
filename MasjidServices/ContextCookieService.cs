using GravyFoodsApi.MasjidRepository;
using Microsoft.AspNetCore.DataProtection;

namespace GravyFoodsApi.MasjidServices
{
    public class ContextCookieService : IContextCookieService
    {
        private readonly IDataProtector _protector;

        public ContextCookieService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("POS_CONTEXT");
        }

        public void SetBranchContext(HttpResponse response, Guid tenantId, Guid branchId)
        {
            var payload = $"{tenantId}|{branchId}";
            var encrypted = _protector.Protect(payload);

            response.Cookies.Append("POS_CONTEXT", encrypted, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
        }
    }

}
