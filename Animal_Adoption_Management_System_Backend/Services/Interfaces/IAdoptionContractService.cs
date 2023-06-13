using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Pagination;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAdoptionContractService : IGenericService<AdoptionContract>
    {
        Task<IEnumerable<AdoptionContract>> GetFilteredAdoptionContractsAsync(string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, bool? isActive);
        Task<AdoptionContract> GetWithDetailsAsync(int id);
        Task<AdoptionContract> TryAddRelatedEntitiesToAdoptionContract(AdoptionContract adoptionContractToCreate, int animalId, string applierId);
        Task<AdoptionContract> UpdateAdoptionContractIsActive(int id, bool isActive);

        Task<AdoptionContract> GetWithAnimalShelterDetailsAsync(int id);
        Task<PagedResult<TResult>> GetPagedAndFilteredAdoptionContractsAsync<TResult>(QueryParameters queryParameters, string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, bool? isActive);
    }
}
