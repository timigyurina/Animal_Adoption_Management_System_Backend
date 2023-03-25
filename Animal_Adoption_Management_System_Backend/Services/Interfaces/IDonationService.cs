using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IDonationService : IGenericRepository<Donation>
    {
        Task<Donation> GetWithDetailsAsync(int id);
        Task<IEnumerable<Donation>> GetFilteredDonationsAsync(string? shelterName, string? donatorName, decimal? minAmount, decimal? maxAmount, DateTime? dateAfter, DateTime? dateBefore, DonationStatus? status);
        Task<Donation> TryAddDonatorAndShelterToDonation(Donation donationToCreate, string userId, int shelterId);
        Task<Donation> UpdateDonationStatus(int id, DonationStatus newStatus);
    }
}
