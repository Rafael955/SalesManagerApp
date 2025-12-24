using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesManagerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesManagerApp.Infra.Data.Mappings
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("ORDER");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasColumnName("ID")
                .IsRequired();

            builder.Property(o => o.OrderDate)
                .HasColumnName("ORDER_DATE")
                .IsRequired();

            builder.Property(o => o.TotalValue)
                .HasColumnName("TOTAL_VALUE")
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(o => o.Status)
                .HasColumnName("STATUS")
                .IsRequired();

            builder.Property(o => o.CustomerId)
                .HasColumnName("CUSTOMER_ID")
                .IsRequired();

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);
        }
    }
}
