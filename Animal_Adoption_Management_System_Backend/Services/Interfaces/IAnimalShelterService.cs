using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAnimalShelterService : IGenericRepository<AnimalShelter>
    {
        Task<AnimalShelter> CreateAnimalShelterConnection(Animal animal, Shelter shelter, DateTime enrollmentDate);
    }
}
