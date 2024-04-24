using IBuyStuff.QueryModel.Customers;
using IBuyStuff.QueryModel.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBuyStuff.QueryModel.Persistence.Mappings
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Primary Key
            builder.HasKey(c => c.CustomerId);
            builder.Property(c => c.CustomerId)
                .IsRequired()
                .HasColumnName("Id");
            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasColumnName("FirstName");
            builder.Property(c => c.LastName)
                .IsRequired()
                .HasColumnName("LastName");
            builder.Property(c => c.Email)
                .IsRequired()
                .HasColumnName("Email");

            //Address
            builder.ComplexProperty<Address>(c => c.Address)
                .Property(a => a.Street)
                .HasMaxLength(30)
                .IsRequired(false)
                .HasColumnName("Address_Street");
            builder.ComplexProperty<Address>(c => c.Address)
                .Property(a => a.City)
                .HasMaxLength(15)
                .IsRequired(false)
                .HasColumnName("Address_City");
            builder.ComplexProperty<Address>(c => c.Address)
                .Property(a => a.Number)
                .IsRequired(false)
                .HasColumnName("Address_Number");
            builder.ComplexProperty<Address>(c => c.Address)
                .Property(a => a.Zip)
                .IsRequired(false)
                .HasMaxLength(15)
                .HasColumnName("Address_Zip");

            //Payment
            builder.ComplexProperty<CreditCard>(c => c.Payment)
                .Property(c => c.Number)
                .IsRequired();
            builder.ComplexProperty<CreditCard>(c => c.Payment)
                .Property(c => c.Owner)
                .IsRequired();
            builder.ComplexProperty<CreditCard>(c => c.Payment)
                .Property(c => c.Type)
                .IsRequired();
            builder.ComplexProperty<CreditCard>(c => c.Payment)
                .ComplexProperty<ExpiryDate>(c => c.Expires)
                .IsRequired();

            // Table and relationships 
            builder.ToTable("Customers");
            builder.HasMany(c => c.Orders);
        }
    }

}