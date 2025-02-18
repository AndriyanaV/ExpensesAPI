using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpensesAPI.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {

            var adminRole = new IdentityRole<int>
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var userRole = new IdentityRole<int>
            {
                Id = 2,
                Name = "User",
                NormalizedName = "USER"
            };

            builder.HasData(adminRole, userRole);
        }
    }
    }

