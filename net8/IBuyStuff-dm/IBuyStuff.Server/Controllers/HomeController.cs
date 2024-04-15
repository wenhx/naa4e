using IBuyStuff.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBuyStuff.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeControllerService _service;

        public HomeController(IHomeControllerService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var model = _service.LayOutHomePage();
            return View(model);
        }

        public ActionResult Subscribe(string  email)
        {
            var model = _service.NewSubscriber(email);
            return View("index", model);
        }
    }
}