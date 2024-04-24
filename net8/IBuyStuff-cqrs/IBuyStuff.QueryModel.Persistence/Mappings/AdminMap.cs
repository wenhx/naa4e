//using IBuyStuff.Domain.Customers;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace IBuyStuff.QueryModel.Persistence.Mappings
//{
//    public class AdminMap : IEntityTypeConfiguration<Admin>
//    {
//        public void Configure(EntityTypeBuilder<Admin> builder)
//        {
//            builder.HasKey(t => t.Name);
//            builder.Property(t => t.Name)
//                .IsRequired()
//                .HasColumnName("Name");
//            builder.ToTable("Admins");
//        }
//    }
//}