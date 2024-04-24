using IBuyStuff.QueryModel.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBuyStuff.QueryModel.Persistence.Mappings
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
