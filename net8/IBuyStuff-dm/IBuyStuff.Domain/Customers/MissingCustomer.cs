namespace IBuyStuff.Domain.Customers
{
    public class MissingCustomer : Customer
    {
        private MissingCustomer() { }
        public static MissingCustomer Instance = new MissingCustomer();
    }
}
