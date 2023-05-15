using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Pagination;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetFilteredUsersAsync(string? name, string? email, bool? isActive, bool? isContactOfShelter, string? shelterName, DateTime? bornAfter, DateTime? bornBefore);

        Task<User> GetWithAllDetailsAsync(string id);
        Task<User> GetWithShelterDetailsAsync(string id);
        Task<User> GetWithDonationDetailsAsync(string id);
        Task<User> GetWithImageDetailsAsync(string id);
        Task<User> GetWithAdoptionApplicationDetailsAsync(string id);
        Task<User> GetWithAdoptionContractDetailsAsync(string id);
        Task<User> GetWithManagedAdoptionContractDetailsAsync(string id);

        Task CreateConnectionWithShelterByEmail(Shelter shelter, string email, bool isContactOfShelter);
        Task<User> UpdateUserIsActive(string id, bool isActive);
        Task<User> UpdateConnectionWithShelterById(Shelter? shelter, string id, bool isContactOfShelter);

        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters, string includeProperties = "");
        Task<PagedResult<TResult>> GetPagedAndFilteredUsersAsync<TResult>(QueryParameters queryParameters, string? name, string? email, bool? isActive, bool? isContactOfShelter, string? shelterName, DateTime? bornAfter, DateTime? bornBefore);
    }
}
