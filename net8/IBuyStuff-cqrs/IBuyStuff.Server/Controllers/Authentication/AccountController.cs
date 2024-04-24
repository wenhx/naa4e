using IBuyStuff.Application.InputModels.Login;
using IBuyStuff.Application.Services;
using IBuyStuff.Server.Common.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IBuyStuff.Server.Controllers.Authentication
{
    public class AccountController : BaseIdentityController
    {
        private readonly ILoginControllerService _service;

        public AccountController(ILoginControllerService service)
        {
            _service = service;
        }

        #region Action:: Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterInputModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterInputModel model)
        {
            // Add customer and sign in
            var succeeded = _service.Register(model);
            if (succeeded)
            {
                var principal = IdentityHelpers.Create(model.UserName, model.Email, model.Gender);
                await HttpContext.SignInAsync(principal, new AuthenticationProperties { IsPersistent = true });
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("failed", "Oh snap! Change a few things up and try again!");
            return View(model);
        }
        #endregion
    }
}