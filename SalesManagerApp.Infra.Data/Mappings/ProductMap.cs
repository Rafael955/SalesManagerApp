using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Infra.Data.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("PRODUCTS");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnName("NAME")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnName("PRICE")
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(p => p.Quantity)
              .HasColumnName("QUANTITY")
              .IsRequired();
        }
    }
}
