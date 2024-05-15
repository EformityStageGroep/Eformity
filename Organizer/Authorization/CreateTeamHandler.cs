using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Organizer.Authorization
{
    public class CreateTeamHandler : AuthorizationHandler<CreateTeamRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateTeamRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "create_team" && c.Value == "true"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
