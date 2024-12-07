using Microsoft.AspNetCore.Authorization;

namespace DigiBanter.Api.AuthorizationHandler
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
