using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Infra.Data.Mappings
{
    public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("ORDER_ITEMS");

            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Id)
                .HasColumnName("ID")
                .IsRequired();

            builder.Property(oi => oi.Quantity)
                .HasColumnName("QUANTITY")
                .IsRequired();

            builder.Property(oi => oi.UnitPrice)
                .HasColumnName("UNIT_PRICE")
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(oi => oi.OrderId)
                .HasColumnName("ORDER_ID")
                .IsRequired();

            builder.Property(oi => oi.ProductId)
                .HasColumnName("PRODUCT_ID")
                .IsRequired();

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.HasOne(oi => oi.Product)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.ProductId);
        }
    }
}
