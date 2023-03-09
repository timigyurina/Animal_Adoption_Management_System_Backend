using Animal_Adoption_Management_System_Backend.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Animal_Adoption_Management_System_Backend.Data
{
    public class AnimalAdoptionContext : IdentityDbContext<User>
    {
        public AnimalAdoptionContext(DbContextOptions options)
           : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<AnimalBreed> AnimalBreeds { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AnimalShelter> AnimalShelters { get; set; }
        public DbSet<AdoptionApplication> AdoptionApplications { get; set; }
        public DbSet<AdoptionContract> AdoptionContracts { get; set; }
        public DbSet<ManagedAdoptionContract> ManagedAdoptionContracts { get; set; }
    }
}
