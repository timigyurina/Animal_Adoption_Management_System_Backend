using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAdoptionContractService : IGenericRepository<AdoptionContract>
    {
        Task<IEnumerable<AdoptionContract>> GetFilteredAdoptionContractsAsync(string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, bool? isActive);
        Task<AdoptionContract> GetWithDetailsAsync(int id);
        Task<AdoptionContract> TryAddRelatedEntitiesToAdoptionContract(AdoptionContract adoptionContractToCreate, int animalId, string applierId);
        Task<AdoptionContract> UpdateAdoptionContractIsActive(int id, bool isActive);
    }
}
