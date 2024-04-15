using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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