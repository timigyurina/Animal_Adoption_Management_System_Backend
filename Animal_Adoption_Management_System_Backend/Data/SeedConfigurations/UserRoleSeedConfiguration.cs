using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animal_Adoption_Management_System_Backend.Data.SeedConfigurations
{
    public class UserRoleSeedConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "abcd1234-82b5-4ef3-95b9-f33f6130d5ac",
                    UserId = "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac"
                }
            );
        }
    }

}
