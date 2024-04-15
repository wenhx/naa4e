﻿using IBuyStuff.Domain.Customers;
using IBuyStuff.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBuyStuff.Persistence.Mappings
{
    public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id");

            // Properties			
            builder.Property(t => t.Quantity)
                .IsRequired()
                .HasColumnName("Quantity");

            // Table and relationships 
            builder.ToTable("OrderItems");
            builder.HasOne(o => o.Order);
            builder.HasOne(o => o.Product);
        }
    }
}
