using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebAPIApplication
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasScopeRequirement requirement
        )
        {
            // If user does not have the scope claim, get out of here
            var scopeClaim = context.User.FindFirst(
                c => c.Type == "scope" && c.Issuer == requirement.Issuer
            );

            if (scopeClaim == null)
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scopes = scopeClaim.Value?.Split(' ');

            // Succeed if the scope array contains the required scope
            if (scopes != null && scopes.Any(s => s == requirement.Scope))
                if (scopes != null && scopes.Any(s => s == requirement.Scope))
                    context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
