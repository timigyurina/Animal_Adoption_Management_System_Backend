using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAdoptionApplicationService : IGenericRepository<AdoptionApplication>
    {
        Task<IEnumerable<AdoptionApplication>> GetFilteredAdoptionApplicationsAsync(string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, ApplicationStatus? status);
        Task<AdoptionApplication> GetWithDetailsAsync(int id);
        Task<AdoptionApplication> TryAddAnimalAndApplierToAdoptionApplication(AdoptionApplication adoptionApplicationToCreate, int animalId, string applierId);
        Task<AdoptionApplication> UpdateAdoptionApplicationStatus(int id, ApplicationStatus newStatus);
        Task SetAdoptionApplicationStatusForContractCreation(Animal animal, User applier);
    }
}
