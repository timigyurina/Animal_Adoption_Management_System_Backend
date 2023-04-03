using Animal_Adoption_Management_System_Backend.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetWithShelterDetailsAsync(string id);
        Task<User> GetWithAllDetailsAsync(string id);
        Task<IEnumerable<User>> GetFilteredUsersAsync(string? name, string? email, bool? isActive, bool? isContactOfShelter, string? shelterName, DateTime? bornAfter, DateTime? bornBefore);

        Task CreateConnectionWithShelterByEmail(Shelter shelter, string email, bool isContactOfShelter);
        Task<User> UpdateUserIsActive(string id, bool isActive);
        Task<User> UpdateConnectionWithShelterById(Shelter? shelter, string id, bool isContactOfShelter);
    }
}
