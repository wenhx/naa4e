﻿using IBuyStuff.Application;
using IBuyStuff.Application.Commands;
using IBuyStuff.Application.Handlers;
using IBuyStuff.Application.InputModels.Order;
using IBuyStuff.Application.Services;
using IBuyStuff.Application.ViewModels;
using IBuyStuff.Application.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IBuyStuff.Server.Controllers.Business
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderControllerService _service;
        private readonly IMemoryCache _cache;
        private readonly ICommandHandler<ProcessOrderBeforePaymentCommand, OrderProcessingViewModel> _beforePaymentCommandHandler;
        private readonly ICommandHandler<ProcessOrderAfterPaymentCommand, OrderProcessedViewModel> _afterPaymentCommandHandler;

        public OrderController(IOrderControllerService service, 
            IMemoryCache cache,
            ICommandHandler<ProcessOrderBeforePaymentCommand, OrderProcessingViewModel> beforePaymentCommandHandler,
            ICommandHandler<ProcessOrderAfterPaymentCommand, OrderProcessedViewModel> afterPaymentCommandHandler)
        {
            _service = service;
            _cache = cache;
            _beforePaymentCommandHandler = beforePaymentCommandHandler;
            _afterPaymentCommandHandler = afterPaymentCommandHandler;
        }

        #region Search task

        [Route("orders")]
        [ActionName("SearchUi")]
        public ActionResult SearchMain()
        {
            return View("search-ui", new ViewModelBase());
        }

        [HttpGet]
        [ActionName("Search")]
        public ActionResult SearchResults(int id)
        {
            var model = _service.RetrieveOrderForCustomer(id);
            return View(model);
        }

        [HttpPost]
        [ActionName("Search")]
        public ActionResult SearchCommand(int id = 0)
        {
            // PRG (Post-Redirect-Get) pattern to avoid F5 refresh issues 
            // (and also key step to neatly separate Commands from Queries)
            return RedirectToAction("Search", new {id = id});
        }

        [HttpGet]
        [ActionName("Last")]
        public ActionResult LastOrderResults()
        {
            var customerId = User.Identity.Name;
            var model = _service.RetrieveLastOrderForCustomer(customerId);
            return View("Search", model);
        }

        [HttpPost]
        [ActionName("Last")]
        public ActionResult LastOrderCommand(int id = 0)
        {
            // PRG (Post-Redirect-Get) pattern to avoid F5 refresh issues 
            // (and also key step to neatly separate Commands from Queries in the future)
            return RedirectToAction("Last"/*, new { id = id }*/);
        }
        #endregion

        #region New Order task
        [HttpGet]
        public ActionResult New()
        {
            var customerId = User.Identity.Name;
            var shoppingCartModel = RetrieveCurrentShoppingCart();
            if (shoppingCartModel == null)
            {
                shoppingCartModel = _service.CreateShoppingCartForCustomer(customerId);
                shoppingCartModel.EnableEditOnShoppingCart = true;
                SaveCurrentShoppingCart(shoppingCartModel);
            }
            return View("shoppingcart", shoppingCartModel);
        }
        #endregion

        #region Add Item task
        [HttpPost]
        [ActionName("AddTo")]
        public ActionResult AddToShoppingCartCommand(int productId, int quantity=1)
        {
            var cart = RetrieveCurrentShoppingCart();
            cart = _service.AddProductToShoppingCart(cart, productId, quantity);
            SaveCurrentShoppingCart(cart);

            // PRG (Post-Redirect-Get) pattern to avoid F5 refresh issues 
            // (and also key step to neatly separate Commands from Queries in the future)
            return RedirectToAction("AddTo");  
        }

        [HttpGet]
        [ActionName("AddTo")]
        public ActionResult DisplayShoppingCartCommand()
        {
            var cart = RetrieveCurrentShoppingCart();
            cart.EnableEditOnShoppingCart = true;
            return View("shoppingcart", cart);
        }

        #endregion

        #region Remove Order Item task

        [HttpGet]
        [ActionName("Remove")]
        public ActionResult RefreshShoppingCart(int itemIndex = -1)
        {
            return DisplayShoppingCartCommand();
        }

        [HttpPost]
        [ActionName("Remove")]
        public ActionResult RemoveItemFromShoppingCart(int itemIndex = -1)
        {
            if (itemIndex < 0)
                return RedirectToAction("Remove");

            var cart = RetrieveCurrentShoppingCart();
            if (itemIndex >= cart.OrderRequest.Items.Count)
                return RedirectToAction("Remove");

            cart.OrderRequest.Items.RemoveAt(itemIndex);
            SaveCurrentShoppingCart(cart);

            // PRG (Post-Redirect-Get) pattern to avoid F5 refresh issues 
            // (and also key step to neatly separate Commands from Queries in the future)
            return RedirectToAction("Remove");
        }

        #endregion

        #region Checkout

        [HttpGet]
        [ActionName("Checkout")]
        public ActionResult DisplayCheckoutPage()
        {
            // Get details: address, payment
            var cart = RetrieveCurrentShoppingCart();
            cart.EnableEditOnShoppingCart = false;
            return View(cart);
        }

        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult Checkout(CheckoutInputModel checkout)
        {
            // Pre-payment steps
            var cart = RetrieveCurrentShoppingCart();
            var command = new ProcessOrderBeforePaymentCommand(cart, checkout);
            var response = _beforePaymentCommandHandler.Handle(command);
            if (!response.Denied)
            {
                return Redirect(Url.Content("~/FakePayment"));
            }
            
            TempData["ibuy-stuff:denied"] = response;
            return RedirectToAction("Denied"); 
        }

        public ActionResult EndCheckout(string tid)
        {
            // Post-payment steps
            var cart = RetrieveCurrentShoppingCart();
            var command = new ProcessOrderAfterPaymentCommand(cart, tid);
            var response = _afterPaymentCommandHandler.Handle(command);
            var action = response.Denied ? "denied" : "processed";
            return View(action, response);
        }

        public ActionResult Denied()
        {
            var model = TempData["ibuy-stuff:denied"] ?? new OrderProcessingViewModel();
            return View(model);
        }

        public ActionResult Processed()
        {
            return View(new OrderProcessedViewModel());
        }
        #endregion

   
        #region Internal members

        private static string GetShoppingCartName(string customerId)
        {
            return String.Format("I-Buy-Stuff-Cart:{0}", customerId);
        }
        private ShoppingCartViewModel RetrieveCurrentShoppingCart()
        {
            var customerId = User.Identity.Name;
            var cartName = GetShoppingCartName(customerId);
            var cart = _cache.Get<ShoppingCartViewModel>(cartName) ?? _service.CreateShoppingCartForCustomer(customerId);
            return cart;
        }
        private void SaveCurrentShoppingCart(ShoppingCartViewModel cart)
        {
            var customerId = User.Identity.Name;
            var cartName = GetShoppingCartName(customerId); 
            _cache.Set(cartName, cart);
        }
        #endregion
    }
}