using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Pagination;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IShelterService : IGenericService<Shelter>
    {
        Task<IEnumerable<Shelter>> GetFilteredSheltersAsync(string? name, string? contactPersonName, bool? isActive);
        Task<Shelter> GetWithAddressAsync(int id);
        Task<Shelter> GetWithDonationsAsync(int id);
        Task<Shelter> GetWithAddressAndAnimalsAsync(int id);
        Task<Shelter> GetWithDetailsAsync(int id);
        Task<Shelter> UpdateShelterIsActive(int id, bool isActive);

        Task<PagedResult<TResult>> GetPagedAndFilteredSheltersAsync<TResult>( QueryParameters queryParameters, string? name, string? contactPersonName, bool? isActive);
    }
}
