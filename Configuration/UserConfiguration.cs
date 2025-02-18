using ExpensesAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesAPI.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)

        {
            var hasher = new PasswordHasher<User>(); 

            var adminUser = new User
            {
                Id = 1,
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123!")
            };

            var normalUser = new User
            {
                Id = 2,
                UserName = "user@example.com",
                NormalizedUserName = "USER@EXAMPLE.COM",
                Email = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                FirstName = "User",
                LastName = "User",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "User123!")
            };

            builder.HasData(adminUser,normalUser);

        }
    }
}
