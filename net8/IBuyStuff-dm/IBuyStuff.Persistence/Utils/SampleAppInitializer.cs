using IBuyStuff.Domain.Customers;
using IBuyStuff.Domain.Orders;
using IBuyStuff.Domain.Products;
using IBuyStuff.Domain.Shared;
using IBuyStuff.Infrastructure;
using IBuyStuff.Persistence.Facade;

namespace IBuyStuff.Persistence.Utils
{
    public class SampleAppInitializer
    {
        public static void Seed(DomainModelFacade context, IHashingService hashingService, Func<Gender, string> avatarService)
        {
            /////////////////////////////////////////////////////////////////
            // Products
            var products = new List<Product>
            {
                new Product(0,"Tennis Racquet", new Money(Currency.Default, 200), 10),
                new Product(0,"Dartboard", new Money(Currency.Default, 10), 10),
                new Product(0,"Volley ball", new Money(Currency.Default, 10), 10),
                new Product(0,"Baseball cap", new Money(Currency.Default, 50), 10),
                new Product(0,"Bike", new Money(Currency.Default, 50), 10),
                new Product(0,"Ice skate shoes", new Money(Currency.Default, 20), 10),
                new Product(0,"Running shoes", new Money(Currency.Default, 60), 10),
                new Product(0,"Basket ball", new Money(Currency.Default, 15), 10),
                new Product(0,"Umbrella", new Money(Currency.Default, 8), 10),
                new Product(0,"Goggles", new Money(Currency.Default, 11), 10),
            };
            context.Products.AddRange(products);

            /////////////////////////////////////////////////////////////////
            // Customers
            var defaultCustomer = Customer.CreateNew(Gender.Male, "naa4e", "Foo", "Bar", "naa4e@expoware.org");
            defaultCustomer.SetPasswordHash(hashingService.Hash("1234abcd"));
            defaultCustomer.SetAvatar(avatarService.Invoke(defaultCustomer.Gender));
            var customers = new List<Customer>
            {
                defaultCustomer
            };
            context.Customers.AddRange(customers);

            /////////////////////////////////////////////////////////////////
            // Admins
            var admins = new List<Admin>
            {
                Admin.CreateNew("admin"),
            };
            context.Admins.AddRange(admins);

            /////////////////////////////////////////////////////////////////
            // Orders
            var orders = new List<Order>
            {
                Order.CreateNew(0, defaultCustomer),
                Order.CreateNew(0, defaultCustomer),
                Order.CreateNew(0, defaultCustomer)
            };
            context.Orders.AddRange(orders);

            /////////////////////////////////////////////////////////////////
            // Fidelity Cards
            var cards = new List<FidelityCard>
            {
                FidelityCard.CreateNew("0101xyz001", defaultCustomer),
            };
            context.FidelityCards.AddRange(cards);
            context.SaveChanges();
        }
    }
}
