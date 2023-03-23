using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AnimalShelterService : GenericRepository<AnimalShelter>, IAnimalShelterService
    {
        public AnimalShelterService(AnimalAdoptionContext context) : base(context)
        {
        }

        public async Task<AnimalShelter> CreateAnimalShelterConnection(Animal animal, Shelter shelter, DateTime enrollmentDate)
        {
            if (ConnectionExists(animal.Id, shelter.Id, enrollmentDate))
                throw new BadRequestException("Connection already exists");

            AnimalShelter connection = new() { Animal = animal, Shelter = shelter, EnrollmentDate = enrollmentDate };
            await AddAsync(connection);
            return connection;
        }

        private bool ConnectionExists(int animalId, int shelterId, DateTime enrollmentDate)
        {
            return _context.AnimalShelters.Any(a => a.Animal.Id == animalId && a.Shelter.Id == shelterId && a.EnrollmentDate >= enrollmentDate);
        }
    }
}
