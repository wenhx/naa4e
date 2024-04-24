﻿using System;
using System.Collections.Generic;
using System.Linq;
using IBuyStuff.Domain.Customers;
using IBuyStuff.Domain.Repositories;
using IBuyStuff.Persistence.Facade;

namespace IBuyStuff.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CommandModelDatabase _db;

        public CustomerRepository(CommandModelDatabase db)
        {
            _db = db;
        }

        public Customer FindById(string id)
        {
            var customer = (from c in _db.Customers where c.CustomerId == id select c).SingleOrDefault();
            if (customer == null)
                return MissingCustomer.Instance;
            return customer;
        }

        #region IRepository MEMBERS

        public IList<Customer> FindAll()
        {
            throw new NotImplementedException();
        }

        public bool Add(Customer aggregate)
        {
            _db.Customers.Add(aggregate);
            return _db.SaveChanges() > 0;
        }

        public bool Save(Customer aggregate)
        {
            try
            {
                var customer = (from c in _db.Customers where c.CustomerId == aggregate.CustomerId select c).Single();
                customer.SetAddress(aggregate.Address);
                customer.SetAvatar(aggregate.Avatar);
                customer.SetPaymentDetails(aggregate.Payment);
                customer.SetPasswordHash(aggregate.PasswordHash);
                var changes = _db.SaveChanges();
                return changes > 0;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool Delete(Customer aggregate)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}