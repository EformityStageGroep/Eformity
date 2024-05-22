using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Organizer.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.Attributes
{
    public static class UserExtensions
    {
        public static async Task<bool> HasRolePropertyAsync(this HttpContext context, string propertyName)
        {
            var roleService = context.RequestServices.GetRequiredService<IRoleService>();
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (userId == null)
            {
                return false;
            }

            var role = await roleService.GetRoleForUserAsync(userId);
            var property = role.GetType().GetProperty(propertyName);
            return property != null && (bool)property.GetValue(role);
        }
    }
}
