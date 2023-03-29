using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IManagedAdoptionContractService : IGenericRepository<ManagedAdoptionContract>
    {
        Task<ManagedAdoptionContract> GetByAdoptionContractIdAsync(int adoptionContractId);
        Task<ManagedAdoptionContract> TryAddRelatedEntitiesToManagedContract(string managerId, AdoptionContract adoptionContract);
    }
}
