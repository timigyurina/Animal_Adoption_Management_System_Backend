using Animal_Adoption_Management_System_Backend.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animal_Adoption_Management_System_Backend.Data.SeedConfigurations
{
    public class UserSeedConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var hasher = new PasswordHasher<User>();

            User adminUser = new()
            {
                Id = "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac",
                Email = "admin@localhost.com",
                NormalizedEmail = "ADMIN@LOCALHOST.COM",
                UserName = "admin@localhost.com",
                NormalizedUserName = "ADMIN@LOCALHOST.COM",
                PasswordHash = hasher.HashPassword(null, "Test123."),
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Admin",
                IsActive = true,
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            builder.HasData(adminUser);
        }
    }
}
