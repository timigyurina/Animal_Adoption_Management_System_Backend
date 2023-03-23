using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAnimalService : IGenericRepository<Animal>
    {
        Task<IEnumerable<Animal>> GetFilteredAnimalsAsync(string? name, string? type, string? size, string? status, string? gender, string? color, int? breedId, bool? isSterilised, DateTime? bornAfter, DateTime? bornBefore);
        Task<Animal> GetWithDetailsAsync(int id);
        Task<Animal> UpdateSterilisation(int id, UpdateSterilisationDTO sterilisationDate);
        Task<Animal> UpdateStatus(int id, AnimalStatus newStatus);
    }
}
