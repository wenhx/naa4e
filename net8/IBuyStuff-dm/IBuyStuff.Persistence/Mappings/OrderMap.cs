using IBuyStuff.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBuyStuff.Persistence.Mappings
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Primary Key
            builder.HasKey(t => t.OrderId);

            // Table and relationships 
            builder.ToTable("Orders");
            builder.HasOne(o => o.Buyer);
            builder.HasMany(o => o.Items);
            builder.ComplexProperty(o => o.Total,
                b => 
                {
                    b.IsRequired();
                    b.ComplexProperty(m => m.Currency).IsRequired();
                });
        }
    }
}