using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AnimalShelterService : GenericRepository<AnimalShelter>, IAnimalShelterService
    {
        public AnimalShelterService(AnimalAdoptionContext context) : base(context)
        {
        }

        public async Task<AnimalShelter> CreateAnimalShelterConnection(Animal animal, Shelter shelter, DateTime enrollmentDate)
        {
            if (ConnectionAlreadyExists(animal.Id, shelter.Id))
                throw new BadRequestException("Connection between Animal and Shelter already exists");

            await CheckForAndCloseConnection(animal, enrollmentDate);

            AnimalShelter connection = new() { Animal = animal, Shelter = shelter, EnrollmentDate = enrollmentDate, ExitDate = null };
            await AddAsync(connection);
            return connection;
        }

        public async Task CheckForAndCloseConnection(Animal animal, DateTime exitDate)
        {
            AnimalShelter? openConnection = await _context.AnimalShelters
                .FirstOrDefaultAsync(a => a.Animal == animal && a.ExitDate == null);
            if (openConnection != null)
            {
                openConnection.ExitDate = exitDate;
                await UpdateAsync(openConnection);
            }
        }

        private bool ConnectionAlreadyExists(int animalId, int shelterId)
        {
            return _context.AnimalShelters
                .Any(a => a.Animal.Id == animalId && a.Shelter.Id == shelterId && a.ExitDate == null);
        }
    }
}
