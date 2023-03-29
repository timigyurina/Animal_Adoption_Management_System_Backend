using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAnimalBreedService : IGenericRepository<AnimalBreed>
    {
        Task<IEnumerable<AnimalBreed>> GetFilteredBreedsAsync(string? name, string? type);
        Task<IEnumerable<AnimalBreed>> GetFilteredBreedsAsync(string? name, AnimalType? t);
    }
}
