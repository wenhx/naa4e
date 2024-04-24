using IBuyStuff.QueryModel.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBuyStuff.Persistence.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ComplexProperty(p => p.UnitPrice, b => 
            {
                b.IsRequired();
                b.ComplexProperty(p => p.Currency).IsRequired();
            });
            builder.ToTable("Products");
        }
    }
}