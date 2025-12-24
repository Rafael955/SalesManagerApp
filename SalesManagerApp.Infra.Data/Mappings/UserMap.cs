using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Enums;

namespace SalesManagerApp.Infra.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USERS");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("ID")
                .IsRequired();

            builder.Property(u => u.Name)
                .HasColumnName("NAME")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasColumnName("PASSWORD")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasColumnName("ROLE")
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasData(new User()
            {
                Id = Guid.Parse("AEF750E0-419E-4A7B-9BF3-34EC41A08744"),
                Name = "Admin",
                Email = "admin@admin.com",
                Password = "6f2cb9dd8f4b65e24e1c3f3fa5bc57982349237f11abceacd45bbcb74d621c25", //Admin@12345
                Role = Role.Admin,
                IsActive = true
            });
        }
    }
}
