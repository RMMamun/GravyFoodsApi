using Microsoft.AspNetCore.DataProtection;

namespace GravyFoodsApi.MasjidServices.RegContext
{
    public class BranchContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDataProtector _protector;

        public BranchContextMiddleware(RequestDelegate next, IDataProtectionProvider provider)
        {
            _next = next;
            _protector = provider.CreateProtector("POS_CONTEXT");
        }

        //public async Task Invoke(HttpContext context)
        //{
        //    Console.WriteLine("BranchContextMiddleware HIT");

        //    if (context.Request.Cookies.TryGetValue("POS_CONTEXT", out var value))
        //    {
        //        Console.WriteLine("POS_CONTEXT cookie FOUND");

        //        try
        //        {
        //            var decrypted = _protector.Unprotect(value);
        //            var parts = decrypted.Split('|');

        //            //context.Items["TenantId"] = Guid.Parse(parts[0]);
        //            //context.Items["BranchId"] = Guid.Parse(parts[1]);

        //            context.Items["TenantId"] = parts[0];
        //            context.Items["BranchId"] = parts[1];
        //        }
        //        catch
        //        {
        //            context.Response.Cookies.Delete("POS_CONTEXT");
        //        }
        //    }
        //    else 
        //    {
        //        Console.WriteLine("POS_CONTEXT cookie NOT FOUND");
        //    }

        //        await _next(context);
        //}
    }

}
