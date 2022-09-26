using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceApp.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Models.EntityConfiguration
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(cr => cr.Id);
            builder.Property(cr => cr.RoleName).IsRequired().HasMaxLength(15);

            builder.HasData(
                new Role {Id=1, RoleName = "Admin"});
            builder.HasData(
                new Role { Id = 2, RoleName = "Customer"});
            builder.HasData(
                new Role { Id = 3, RoleName = "Support"});
            builder.HasData(
                new Role { Id = 4, RoleName = "Developer"});
        }
    }
}
