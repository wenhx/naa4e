using IBuyStuff.QueryModel.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IBuyStuff.QueryModel.Persistence.Mappings
{
    public class ExpiryDateMap : IEntityTypeConfiguration<ExpiryDate>
    {
        public void Configure(EntityTypeBuilder<ExpiryDate> builder)
        {
            builder.HasNoKey();
            builder.Ignore(d => d.When);
        }
    }
}
