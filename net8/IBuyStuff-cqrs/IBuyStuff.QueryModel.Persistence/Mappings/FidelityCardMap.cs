//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Microsoft.EntityFrameworkCore;

//namespace IBuyStuff.QueryModel.Persistence.Mappings
//{
//    public class FidelityCardMap : IEntityTypeConfiguration<FidelityCard>
//    {
//        public void Configure(EntityTypeBuilder<FidelityCard> builder)
//        {
//            builder.HasKey(c => c.Number);
//            builder.Property(c => c.Number)
//                .IsRequired()
//                .HasColumnName("Number");
//            builder.ToTable("FidelityCards");
//            builder.HasOne(c => c.Owner);
//        }
//    }
//}
