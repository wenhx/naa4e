using IBuyStuff.Persistence.Mappings;
using IBuyStuff.QueryModel.Orders;
using IBuyStuff.QueryModel.Persistence.Mappings;
using IBuyStuff.QueryModel.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace IBuyStuff.QueryModel.Persistence
{
    public class QueryModelDatabase : DbContext, IQueryModelDatabase
    {
        private readonly DbSet<Order> _orders;
        private readonly DbSet<Product> _products;

        public QueryModelDatabase(DbContextOptions<QueryModelDatabase> options) : base(options)
        {
            _products = base.Set<Product>();
            _orders = base.Set<Order>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new OrderItemMap());
            modelBuilder.ApplyConfiguration(new CurrencyMap());
            modelBuilder.ApplyConfiguration(new MoneyMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new CustomerMap());
            //modelBuilder.ApplyConfiguration(new ExpiryDateMap());
            //modelBuilder.ApplyConfiguration(new FidelityCardMap());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove<CascadeDeleteConvention>();
        }

        public IQueryable<Order> Orders
        {
            get { return _orders.Include(o => o.Items).ThenInclude(i => i.Product); }
        }

        public IQueryable<Product> Products
        {
            get { return _products; }
        }
    }
}