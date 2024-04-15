using Microsoft.AspNetCore.Mvc;

namespace IBuyStuff.Server.Controllers.Authentication
{
    public class BaseIdentityController : Controller
    {
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
	}
}