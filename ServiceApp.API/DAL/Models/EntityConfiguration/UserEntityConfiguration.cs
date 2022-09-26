using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceApp.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Models.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.ToTable("Users");
        builder.HasKey(k => k.Id);
        builder.HasDiscriminator<int>("UserType")
            .HasValue<User>(1)
            .HasValue<Customer>(2);
        builder.Property(u => u.FirstName).HasMaxLength(30);
        builder.Property(u => u.LastName).HasMaxLength(30);
        builder.Property(u => u.Email).HasMaxLength(50);

        builder.ToTable("User");
        builder.HasKey(c => c.Id);

        builder.HasOne(a => a.JiraCredentials)
            .WithOne(b => b.User)
            .HasForeignKey<JiraCredentials>(b => b.Id);

        builder.HasData(new User
            {
                Id = Guid.Parse("EF35F28A5C2D41ABBADC1B120C33583C"),
                FirstName = "John",
                LastName = "Test",
                Email = "John@gmail.com",
                PasswordSalt = new byte[] { 242, 36, 213, 46, 174, 0, 248, 143, 252, 58, 211, 207, 3, 59, 188, 127, 158, 232, 185, 250, 204, 142, 75, 22, 155, 217, 0, 41, 244, 23, 190, 184, 167, 238, 198, 220, 33, 170, 126, 144, 213, 103, 209, 175, 56, 61, 206, 219, 207, 100, 80, 76, 24, 45, 11, 253, 63, 167, 251, 192, 121, 228, 209, 220, 71, 233, 168, 2, 30, 182, 39, 80, 241, 52, 115, 230, 91, 155, 102, 126, 15, 192, 184, 93, 248, 143, 173, 35, 5, 140, 177, 42, 9, 46, 4, 17, 98, 209, 111, 50, 87, 139, 3, 218, 15, 174, 65, 144, 169, 231, 40, 55, 103, 253, 31, 28, 83, 71, 9, 213, 238, 181, 128, 25, 57, 160, 42, 42 },
                PasswordHash = new byte[] { 198, 5, 225, 182, 225, 60, 36, 164, 30, 111, 15, 20, 30, 49, 246, 65, 213, 83, 37, 119, 255, 44, 250, 177, 37, 129, 195, 138, 41, 145, 255, 123, 149, 110, 25, 132, 97, 58, 15, 33, 135, 245, 192, 219, 152, 201, 40, 186, 192, 78, 162, 68, 252, 70, 23, 229, 121, 13, 103, 79, 47, 86, 150, 203 },
                IsActive = true,
            });
    }
}

