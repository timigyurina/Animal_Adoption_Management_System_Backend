using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using System.Security.Claims;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAdoptionApplicationService : IGenericService<AdoptionApplication>
    {
        Task<IEnumerable<AdoptionApplication>> GetFilteredAdoptionApplicationsAsync(string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, ApplicationStatus? status);
        Task<AdoptionApplication> GetWithDetailsAsync(int id);
        Task<AdoptionApplication> TryAddAnimalAndApplierToAdoptionApplication(int animalId, ClaimsPrincipal applier);
        Task<AdoptionApplication> UpdateAdoptionApplicationStatus(int id, ApplicationStatus newStatus);
        Task SetAdoptionApplicationStatusForContractCreation(Animal animal, User applier);

        Task<AdoptionApplication> GetWithAnimalShelterDetailsAsync(int id);
        Task<PagedResult<TResult>> GetPagedAndFilteredAdoptionApplicationsAsync<TResult>(QueryParameters queryParameters, string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, ApplicationStatus? status);
    }
}
