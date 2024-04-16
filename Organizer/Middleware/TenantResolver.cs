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
            var tenantClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/tenantid"); // Retrieve tid claim

            if (tenantClaim != null)
            {
                currentTenantService.TenantId = tenantClaim.Value; // Set TenantId based on tid claim
            }

            await _next(context);
        }
    }
}
