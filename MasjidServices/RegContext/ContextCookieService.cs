using GravyFoodsApi.MasjidRepository;
using Microsoft.AspNetCore.DataProtection;

namespace GravyFoodsApi.MasjidServices.RegContext
{
    public class ContextCookieService : IContextCookieService
    {
        private readonly IDataProtector _protector;

        public ContextCookieService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("POS_CONTEXT");
        }

        public void SetBranchContext(HttpResponse response, string tenantId, string branchId)
        {
            //public void SetBranchContext(HttpResponse response, Guid tenantId, Guid branchId)

            try
            {
                var payload = $"{tenantId}|{branchId}";
                var encrypted = _protector.Protect(payload);

                response.Cookies.Append("POS_CONTEXT", encrypted, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                throw new InvalidOperationException("Failed to set branch context cookie.", ex);
            }
        }
    }

}
