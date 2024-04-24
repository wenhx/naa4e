using System;
using System.Collections.Generic;
using IBuyStuff.QueryModel.Orders;
using IBuyStuff.QueryModel.Shared;


namespace IBuyStuff.Application.ViewModels.Orders
{
    public class OrderFoundViewModel : ViewModelBase
    {
        private readonly IList<OrderItem> _orderItems = new List<OrderItem>();

        public static OrderFoundViewModel CreateFromOrder(Order order)
        {
            if (order == null)
                return new OrderNotFoundViewModel();

            var model = new OrderFoundViewModel()
            {
                Id = order.OrderId,
                State = order.State.ToString(),
                OrderDate = order.Date,
                Total = order.Total
            };
            foreach (var item in order.Items)
            {
                model._orderItems.Add(item);
            }
            return model;
        }

        public int Id { get; set; }
        public string State { get; set; }
        public DateTime OrderDate { get; set; }
        public Money Total { get; set; }
        public IEnumerable<OrderItem> Details 
        {
            get 
            { 
                return _orderItems;
            }
        }
    }
}