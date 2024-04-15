using IBuyStuff.Application.InputModels.Login;
using IBuyStuff.Application.Services;
using IBuyStuff.Application.Utils;
using IBuyStuff.Application.ViewModels.Login;
using IBuyStuff.Domain.Customers;
using IBuyStuff.Server.Common.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IBuyStuff.Server.Controllers.Authentication
{
    public class LoginController : BaseIdentityController 
    {
        private const string TwitterScheme = "Twitter";
        private const string FacebookScheme = "Facebook";
        private readonly ILoginControllerService _service;

        public LoginController(ILoginControllerService service)
        {
            _service = service;
        }


        #region Action:: SignIn
        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public ActionResult SignIn(String returnUrl)
        {
            var model = new LoginInputModel {ReturnUrl = returnUrl, RememberMe = true};
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(LoginInputModel model, String returnUrl)
        {
            // Validate credentials
            var customer = _service.ValidateAndReturn(model);
            if (customer != null)
            {
                var principal = IdentityHelpers.Create(customer.CustomerId, customer.Email, customer.Gender, customer.Avatar);
                await HttpContext.SignInAsync(principal, new AuthenticationProperties { IsPersistent = true });
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("failed", "Oh snap! Change a few things up and try again!");
            return View(model);
        }
        #endregion

        #region Action:: SignOut
        [Route("logout")]
        public new ActionResult SignOut()
        {
            return base.SignOut(new AuthenticationProperties { RedirectUri = "/" },
                IdentityConstants.ExternalScheme, 
                IdentityConstants.ApplicationScheme);
        }
        #endregion

        #region Action:: Twitter 
        [AllowAnonymous]
        public ActionResult Twitter(String returnUrl)
        {
            var returnUrl2 = Url.Action("TwitterLoginCallback", "Login", new { ReturnUrl = returnUrl });
            return new ChallengeResult(TwitterScheme, new AuthenticationProperties { RedirectUri = returnUrl2 });
        }

        public async Task<ActionResult> TwitterLoginCallback(string returnUrl)
        {
            var loginInfo = await _service.GetExternalLoginInfoAsync(TwitterScheme);
            if (loginInfo == null)
            {
                return RedirectToAction("SignIn");
            }             

            return RedirectToAction("AddDetails", "Login",
                new
                {
                    FirstName = "",
                    LastName = "",
                    ReturnUrl = returnUrl,
                    Email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                    UserName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name)
                });
        }
       
        #endregion

        #region Action:: Facebook
        [AllowAnonymous]
        public ActionResult Fb(String returnUrl)
        {
            var returnUrl2 = Url.Action("FacebookLoginCallback", "Login", new { ReturnUrl = returnUrl });
            return new ChallengeResult(FacebookScheme, new AuthenticationProperties { RedirectUri = returnUrl2 });
        }

        [AllowAnonymous]
        public async Task<ActionResult> FacebookLoginCallback(string returnUrl)
        {
            var loginInfo = await _service.GetExternalLoginInfoAsync(FacebookScheme);
            if (loginInfo == null)
            {
                return RedirectToAction("SignIn");
            }

            var fbId = loginInfo.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var first = String.Empty;
            var last = String.Empty;
            var claimName = loginInfo.Principal.Claims.SingleOrDefault(c => c.Type == "urn:facebook:name");
            if (claimName != null)
            {
                var fbName = claimName.Value;
                if (!fbName.IsNullOrEmpty())
                {
                    var tokens = fbName.Split(' ');
                    if (tokens.Length > 0)
                    {
                        first = tokens[0].Trim();
                        last = fbName.Replace(first, "").Trim();
                    }
                }
            }

            return RedirectToAction("AddDetails", "Login",
                new
                {
                    FirstName = first,
                    LastName = last,
                    ReturnUrl = returnUrl,
                    Email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                    UserName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name),
                    Avatar = String.Format("https://graph.facebook.com/{0}/picture?type=large", fbId)
                });
        }
        #endregion

        #region Action:: Finalize external login

        [AllowAnonymous]
        [ActionName("AddDetails")]
        [HttpGet]
        public ActionResult AddDetailsGet(AddDetailsViewModel model)
        {
            return View(model);
        }

        [AllowAnonymous]
        [ActionName("AddDetails")]
        [HttpPost]
        public async Task<ActionResult> AddDetailsPost(AddDetailsViewModel model)
        {
            // Create a user too if missing
            var customer = _service.GetCustomer(model.UserName);
            if (customer == MissingCustomer.Instance)
            {
                var register = new RegisterInputModel()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    Avatar = model.Avatar,
                    Password = Guid.NewGuid().ToString()
                };
                _service.Register(register);
            }

            var principal = IdentityHelpers.Create(model.UserName, model.Email, model.Gender, model.Avatar);
            await HttpContext.SignInAsync(principal, new AuthenticationProperties { IsPersistent = true });
            return RedirectToLocal(model.ReturnUrl);
        }

        #endregion
    }
}