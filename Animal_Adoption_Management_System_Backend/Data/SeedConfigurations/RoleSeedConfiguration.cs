using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animal_Adoption_Management_System_Backend.Data.SeedConfigurations
{
    public class RoleSeedConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "abcd1234-82b5-4ef3-95b9-f33f6130d5ac",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Id = "aaaa1111-82b5-4ef3-95b9-f33f6130d5ac",
                    Name = "ShelterEmployee",
                    NormalizedName = "SHELTEREMPLOYEE"
                },
                new IdentityRole
                {
                    Id = "bbbb2222-82b5-4ef3-95b9-f33f6130d5ac",
                    Name = "Adopter",
                    NormalizedName = "ADOPTER"
                }
            );
        }
    }

}
