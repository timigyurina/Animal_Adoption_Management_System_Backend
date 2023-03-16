using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AnimalBreedService : GenericRepository<AnimalBreed>, IAnimalBreedService
    {
        public AnimalBreedService(AnimalAdoptionContext context) : base(context)
        {
        }
    }
}
