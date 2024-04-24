using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace IBuyStuff.Application.Services.Authentication
{
    public class DefaultSecurityStampValidator : ISecurityStampValidator
    {
        public Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            return Task.FromResult(0);
        }
    }
}
