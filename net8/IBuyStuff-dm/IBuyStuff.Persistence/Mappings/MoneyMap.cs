using IBuyStuff.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBuyStuff.Persistence.Mappings
{
    public class MoneyMap : IEntityTypeConfiguration<Money>
    {
        public void Configure(EntityTypeBuilder<Money> builder)
        {
            builder.HasNoKey();
            builder.ComplexProperty(m => m.Currency)
                .IsRequired();
        }
    }
}
