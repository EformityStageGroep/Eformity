using Microsoft.AspNetCore.Http;
using Organizer.Services;
using System.Linq;

namespace Organizer.Middleware
{
    public class TenantResolver
    {
        private readonly RequestDelegate _next;

        public TenantResolver(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentTenantService currentTenantService)
        {
            var tenantClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/tenant_id"); // Retrieve tid claim

            if (tenantClaim != null)
            {
                currentTenantService.tenant_id = tenantClaim.Value; // Set tenant_id based on tid claim
            }

            await _next(context);
        }
    }
}
