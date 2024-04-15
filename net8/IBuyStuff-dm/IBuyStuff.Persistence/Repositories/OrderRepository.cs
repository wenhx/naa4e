using IBuyStuff.Domain.Orders;
using IBuyStuff.Domain.Repositories;
using IBuyStuff.Persistence.Facade;
using Microsoft.EntityFrameworkCore;

namespace IBuyStuff.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DomainModelFacade _db;

        public OrderRepository(DomainModelFacade db) 
        { 
            _db = db;
        }

        #region Specific members

        public Order FindById(int id)
        {
            try
            {
                // Need to load the entire graph of objects
                var order = (from o in _db.Orders
                    .Include("Items")
                    .Include("Items.Product")
                             where o.OrderId == id
                             select o).Single();
                return order;
            }
            catch (InvalidOperationException)
            {
                return new MissingOrder();
            }
        }

        public Order FindLastByCustomer(string customerId)
        {
            try
            {
                // Need to load the entire graph of objects
                var order = (from o in _db.Orders
                    .Include("Buyer")
                    .Include("Items")
                    .Include("Items.Product")
                             where o.Buyer.CustomerId == customerId
                             orderby o.OrderId descending
                             select o).First();
                return order;
            }
            catch (InvalidOperationException)
            {
                return new MissingOrder();
            }
        }

        public int AddAndReturnKey(Order aggregate)
        {
            _db.Entry(aggregate.Buyer).State = EntityState.Unchanged;
            _db.Orders.Add(aggregate);
            if (_db.SaveChanges() > 0)
            {
                return aggregate.OrderId;
            }
            return 0;
        }
        #endregion

        #region IRepository MEMBERS

        public IList<Order> FindAll()
        {
            var orders = (from o in _db.Orders select o).ToList();
            return orders;
        }

        public bool Save(Order aggregate)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Order aggregate)
        {
            throw new NotImplementedException();
        }

        #endregion


        public bool Add(Order aggregate)
        {
            throw new NotImplementedException();
        }
    }
}