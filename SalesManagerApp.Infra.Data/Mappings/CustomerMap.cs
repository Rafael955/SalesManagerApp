using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesManagerApp.Domain.Entities;

namespace SalesManagerApp.Infra.Data.Mappings
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("CUSTOMERS");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("ID")
                .IsRequired();

            builder.Property(c => c.Name)
                .HasColumnName("NAME")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Phone)
                .HasColumnName("PHONE")
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

            builder.HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
