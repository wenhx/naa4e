using IBuyStuff.Application.InputModels.Login;
using IBuyStuff.Application.Utils;
using IBuyStuff.Domain.Customers;
using IBuyStuff.Domain.Repositories;
using IBuyStuff.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IBuyStuff.Application.Services.Authentication
{
    public class LoginControllerService : ILoginControllerService
    {
        private const string AuthSchemeKey = ".AuthScheme";
        private const string XsrfKey = "XsrfId";

        private readonly ICustomerRepository _customerRepository;
        private readonly IHashingService _hashingService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationSchemeProvider _schemes;

        public LoginControllerService(ICustomerRepository customerRepository, IHashingService hashingService,
            IHttpContextAccessor accessor, IAuthenticationSchemeProvider schemes)
        {
            _customerRepository = customerRepository;
            _hashingService = hashingService;
            _httpContextAccessor = accessor;
            _schemes = schemes;
        }

        public Customer ValidateAndReturn(LoginInputModel model)
        {
            var customer = _customerRepository.FindById(model.UserName);
            if (customer != null)
            {
                if (model.Password.IsNullOrEmpty())
                    return customer;
                if (_hashingService.Validate(model.Password, customer.PasswordHash))
                    return customer;
            } 
            return null;
        }

        public Customer GetCustomer(string userName)
        {
            return _customerRepository.FindById(userName);
        }

        public bool Register(RegisterInputModel model)
        {
            if (model.IsValid())
                return false;

            var customer = Customer.CreateNew(model.Gender, model.UserName, model.FirstName, model.LastName, model.Email);
            customer.SetAvatar(model.Avatar);
            var hash = _hashingService.Hash(model.Password);
            customer.SetPasswordHash(hash);
            return _customerRepository.Add(customer);
        }

        public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string scheme, string? expectedXsrf = null)
        {
            var auth = await _httpContextAccessor.HttpContext.AuthenticateAsync(scheme);
            var items = auth?.Properties?.Items;
            if (auth?.Principal == null || items == null || !items.TryGetValue(AuthSchemeKey, out var provider))
            {
                return null;
            }

            if (expectedXsrf != null)
            {
                if (!items.TryGetValue(XsrfKey, out var userId) ||
                    userId != expectedXsrf)
                {
                    return null;
                }
            }

            var providerKey = auth.Principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? auth.Principal.FindFirstValue("sub");
            if (providerKey == null || provider == null)
            {
                return null;
            }

            var providerDisplayName = (await GetExternalAuthenticationSchemesAsync()).FirstOrDefault(p => p.Name == provider)?.DisplayName
                                        ?? provider;
            return new ExternalLoginInfo(auth.Principal, provider, providerKey, providerDisplayName)
            {
                AuthenticationTokens = auth.Properties?.GetTokens(),
                AuthenticationProperties = auth.Properties
            };
        }

        private async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            var schemes = await _schemes.GetAllSchemesAsync();
            return schemes.Where(s => !string.IsNullOrEmpty(s.DisplayName));
        }
    }
}