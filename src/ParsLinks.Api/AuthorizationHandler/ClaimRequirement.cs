using Microsoft.AspNetCore.Authorization;

namespace ParsLinks.Api.AuthorizationHandler
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public ClaimRequirement(string value)
        {
            ClaimValue = value;
        }
        public string ClaimValue { get; set; }
    }
}
