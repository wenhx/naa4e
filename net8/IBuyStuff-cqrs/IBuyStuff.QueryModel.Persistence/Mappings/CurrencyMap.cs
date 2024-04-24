using IBuyStuff.QueryModel.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBuyStuff.QueryModel.Persistence.Mappings
{
    public class CurrencyMap : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasNoKey();
            builder.Ignore(c => c.Name);
            builder.Property(c => c.Symbol)
                .IsRequired()
                .HasColumnName("Symbol");
        }
    }
}