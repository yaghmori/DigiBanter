using DigiBanter.Shared.Constatns;
using Microsoft.AspNetCore.Authorization;

namespace DigiBanter.Api.AuthorizationHandler
{

    public class ClaimRequirementHandler : AuthorizationHandler<ClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            try
            {

                // Check if the user has the required claim
                if (context.User.HasClaim(AppClaimTypes.auth, requirement.ClaimValue))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                context.Fail();
                return Task.CompletedTask;
            }
        }
    }



}
