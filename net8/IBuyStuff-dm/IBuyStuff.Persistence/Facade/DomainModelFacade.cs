using IBuyStuff.Domain.Customers;
using IBuyStuff.Domain.Misc;
using IBuyStuff.Domain.Orders;
using IBuyStuff.Domain.Products;
using IBuyStuff.Domain.Shared;
using IBuyStuff.Persistence.Mappings;
using IBuyStuff.Persistence.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace IBuyStuff.Persistence.Facade
{
    public class DomainModelFacade : DbContext
    {
        public DomainModelFacade(DbContextOptions options) : base(options)
        {
            Products = base.Set<Product>();
            Customers = base.Set<Customer>();
            Orders = base.Set<Order>();
            OrderItems = base.Set<OrderItem>();
            Admins = base.Set<Admin>();
            FidelityCards = base.Set<FidelityCard>();
            Subscribers = base.Set<Subscriber>();            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExpiryDateMap());
            modelBuilder.ApplyConfiguration(new FidelityCardMap());
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new CurrencyMap());
            modelBuilder.ApplyConfiguration(new OrderItemMap());
            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new AdminMap());
            modelBuilder.ApplyConfiguration(new MoneyMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove<CascadeDeleteConvention>();
        }

        public DbSet<Order> Orders { get; private set; }

        public DbSet<OrderItem> OrderItems { get; private set; }

        public DbSet<Customer> Customers { get; private set; }

        public DbSet<Admin> Admins { get; private set; }

        public DbSet<Product> Products { get; private set; }

        public DbSet<FidelityCard> FidelityCards { get; private set; }

        public DbSet<Subscriber> Subscribers { get; private set; }
    }
}