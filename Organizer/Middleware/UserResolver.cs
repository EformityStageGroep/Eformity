using Microsoft.AspNetCore.Http;
using Organizer.Services;
using System.Linq;

namespace Organizer.Middleware
{
    public class UserResolver
    {
        private readonly RequestDelegate _next;

        public UserResolver(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
        {
            var UserClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"); // Retrieve tid claim

            if (UserClaim != null)
            {
                currentUserService.user_id = UserClaim.Value; // Set user_id based on tid claim
            }

            await _next(context);
        }
    }
}
