using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IShelterService : IGenericRepository<Shelter>
    {
        Task<IEnumerable<Shelter>> GetFilteredSheltersAsync(string? name, string? contactPersonName, bool? isActive);
        Task<Shelter> GetWithAddressAsync(int id);
        Task<Shelter> GetWithDetailsAsync(int id);
        Task<Shelter> TryAddContactPersonToShelter(Shelter shelterToCreate, string contactPersonId);
        Task<Shelter> UpdateShelterIsActive(int id, bool isActive);
        Task<Shelter> UpdateShelterContactPerson(int id, string contactPersonId);
    }
}
