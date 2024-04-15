using System.Security.Claims;
using IBuyStuff.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace IBuyStuff.Server.Common.Identity
{
    public class IdentityHelpers
    {
        public const string IBuyStuff_Avatar = "urn:ibuystuff:avatar";
        public static ClaimsPrincipal Create(string name, string email, Gender gender, string avatar = null)
        {
            avatar = avatar ?? GetAvatar(gender);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                new Claim(IBuyStuff_Avatar, avatar)
            };

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            return new ClaimsPrincipal(identity);
        }

        public static string GetAvatar(Gender gender) 
        { 
            return String.Format("~/content/images/main/{0}.png", gender);
        }
    }
}