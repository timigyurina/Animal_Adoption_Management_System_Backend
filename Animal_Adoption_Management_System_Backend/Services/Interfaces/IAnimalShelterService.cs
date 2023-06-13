using Animal_Adoption_Management_System_Backend.Models.Entities;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAnimalShelterService : IGenericService<AnimalShelter>
    {
        Task CheckForAndCloseConnection(Animal animal, DateTime exitDate);
        Task<AnimalShelter> CreateAnimalShelterConnection(Animal animal, Shelter shelter, DateTime enrollmentDate);
    }
}
