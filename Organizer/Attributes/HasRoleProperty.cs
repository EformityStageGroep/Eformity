using Organizer.Services;

namespace Organizer.Attributes
{
    public static class UserExtensions
    {
        public static async Task<bool> HasRolePropertyAsync(this HttpContext context, string propertyName)
        {
            var userService = context.RequestServices.GetRequiredService<IUserService>();
            var roleService = context.RequestServices.GetRequiredService<IRoleService>();
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (userId == null)
            {
                return false;
            }
            var user = await userService.GetUserInfo();
            Console.WriteLine("testuser " + user.UserId);
            var role = await roleService.GetRoleForUserAsync(user.UserId);
            var property = role.GetType().GetProperty(propertyName);
            return property != null && (bool)property.GetValue(role);
        }
    }
}
