using System.Security.Claims;
using System.Security.Principal;

namespace IBuyStuff.Server.Common.Identity
{
    public static class IdentityExtensions
    {
        public static string? Email(this ClaimsIdentity identity)
        {
            return identity.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
        }

        public static string? Avatar(this ClaimsIdentity identity)
        {
            return identity.FindFirst(c => c.Type == IdentityHelpers.IBuyStuff_Avatar)?.Value;
        }

        public static ClaimsIdentity AsClaimsIdentity(this IIdentity identity)
        {
            return (ClaimsIdentity) identity;
        }
    }
}