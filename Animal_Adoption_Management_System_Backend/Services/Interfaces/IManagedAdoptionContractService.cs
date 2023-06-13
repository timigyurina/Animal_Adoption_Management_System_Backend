using Animal_Adoption_Management_System_Backend.Models.Entities;
using System.Security.Claims;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IManagedAdoptionContractService : IGenericService<ManagedAdoptionContract>
    {
        Task<ManagedAdoptionContract> GetByAdoptionContractIdAsync(int adoptionContractId);
        Task<ManagedAdoptionContract> TryAddRelatedEntitiesToManagedContract(ClaimsPrincipal manager, AdoptionContract adoptionContract);
    }
}
