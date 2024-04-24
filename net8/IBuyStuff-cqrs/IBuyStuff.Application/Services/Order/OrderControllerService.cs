using IBuyStuff.Application.ViewModels.Orders;
using IBuyStuff.Domain.Orders;
using IBuyStuff.Domain.Repositories;
using IBuyStuff.Domain.Services;
using IBuyStuff.Domain.Services.Impl;
using IBuyStuff.Persistence.Repositories;
using IBuyStuff.QueryModel.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IBuyStuff.Application.Services.Order
{
    public class OrderControllerService : IOrderControllerService
    {
        private readonly ICatalogService _catalogService;
        private readonly QueryModelDatabase _db;

        public OrderControllerService(QueryModelDatabase db, ICatalogService catalogService)
        {
            _db = db;
            _catalogService = catalogService;
        }

        // TO BE MODIFIED (LET)
        public OrderFoundViewModel RetrieveOrderForCustomer(int orderId)
        {
            var order = _db.Orders
                            .Where(o => o.OrderId == orderId)
                            .FirstOrDefault();
            if (order == null) 
                return new OrderNotFoundViewModel();

            return OrderFoundViewModel.CreateFromOrder(order);
        }

        public OrderFoundViewModel RetrieveLastOrderForCustomer(string customerId)
        {
            var order = _db.Orders
                .Where(o => o.Buyer.CustomerId == customerId)
                .OrderByDescending(o => o.Date).FirstOrDefault();
            if (order == null)
                return new OrderNotFoundViewModel();
            return OrderFoundViewModel.CreateFromOrder(order);
        }

        public ShoppingCartViewModel CreateShoppingCartForCustomer(string customerId)
        {
            var adHocCatalog = _catalogService.GetCustomerAdHocCatalog(customerId);
            var cart = ShoppingCart.CreateEmpty(adHocCatalog.Customer);
            return ShoppingCartViewModel.CreateEmpty(cart, adHocCatalog.Products);
        }

        public ShoppingCartViewModel AddProductToShoppingCart(ShoppingCartViewModel cart, int productId, int quantity)
        {
            var product = (from p in cart.Products where p.Id == productId select p).First();
            cart.OrderRequest.AddItem(quantity, product);
            return cart;
        }
    }
}