using IBuyStuff.Application.InputModels.Login;
using IBuyStuff.Domain.Customers;
using Microsoft.AspNetCore.Identity;

namespace IBuyStuff.Application.Services
{
    public interface ILoginControllerService
    {
        Customer ValidateAndReturn(LoginInputModel model);
        Customer GetCustomer(string userName);
        bool Register(RegisterInputModel model);
        Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string scheme, string? expectedXsrf = null);
    }
}