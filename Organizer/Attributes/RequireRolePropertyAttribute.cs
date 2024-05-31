using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Graph;
using Organizer.Services;
using System;
using System.Threading.Tasks;

namespace Organizer.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequireRolePropertyAttribute : ActionFilterAttribute
    {
        private readonly string _propertyName;




        public RequireRolePropertyAttribute(string propertyName)
        {
            _propertyName = propertyName;
            
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var roleService = context.HttpContext.RequestServices.GetRequiredService<IRoleService>();
            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;


            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var role = await roleService.GetRoleForUserAsync(userId);
            var property = role.GetType().GetProperty(_propertyName);
            if (property == null || !(bool)property.GetValue(role))
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}
