using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAnimalService : IGenericRepository<Animal>
    {
        Task<IEnumerable<Animal>> GetFilteredAnimalsAsync(string? name, AnimalType? type, AnimalSize? size, AnimalStatus? status, Gender? gender, AnimalColor? color, int? breedId, bool? isSterilised, DateTime? bornAfter, DateTime? bornBefore);
        Task<Animal> GetWithInfoForAdoptersAsync(int id);
        Task<Animal> GetWithDetailsAsync(int id);
        Task<Animal> GetWithImagesAsync(int id);
        Task<Animal> UpdateSterilisation(int id, UpdateSterilisationDTO sterilisationDate);
        Task<Animal> UpdateStatus(int id, AnimalStatus newStatus);
        Task<Animal> GetWithAnimalShelterDetailsAsync(int animalId);
        Task<PagedResult<TResult>> GetPagedAndFilteredAnimalsAsync<TResult>(QueryParameters queryParameters, string? name, AnimalType? type, AnimalSize? size, AnimalStatus? status, Gender? gender, AnimalColor? color, int? breedId, bool? isSterilised, DateTime? bornAfter, DateTime? bornBefore);
        AnimalShelter GetLatestShelterConnectionOfAnimal(Animal animalWithDetails);
    }
}
