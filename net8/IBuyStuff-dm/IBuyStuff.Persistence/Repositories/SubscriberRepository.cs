using System.Collections.Generic;
using System.Linq;
using IBuyStuff.Domain.Misc;
using IBuyStuff.Domain.Repositories;
using IBuyStuff.Persistence.Facade;

namespace IBuyStuff.Persistence.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly DomainModelFacade _db;

        public SubscriberRepository(DomainModelFacade db) 
        { 
            _db = db;
        }

        public int Count()
        {
            return (from s in _db.Subscribers select s).Count();
        }

        #region IRepository
        public IList<Subscriber> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public bool Add(Subscriber aggregate)
        {
            _db.Subscribers.Add(aggregate);
            return _db.SaveChanges() > 0;
        }

        public bool Save(Subscriber aggregate)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Subscriber aggregate)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}