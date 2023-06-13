using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class DonationService : GenericService<Donation>, IDonationService
    {
        public DonationService(AnimalAdoptionContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Donation> GetWithDetailsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(Donation).Name, id);

            return await _context.Donations
                .Include(d => d.Donator)
                .Include(d => d.Shelter)
                .AsNoTracking()
                .FirstAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Donation>> GetFilteredDonationsAsync(string? shelterName, string? donatorName, decimal? minAmount, decimal? maxAmount, DateTime? dateAfter, DateTime? dateBefore, DonationStatus? status)
        {
            IQueryable<Donation> donationQuery = _context.Donations
                .Include(d => d.Shelter)
                .Include(d => d.Donator)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(shelterName))
            {
                donationQuery = donationQuery.Where(d => d.Shelter.Name.ToLower().Contains(shelterName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(donatorName))
            {
                donationQuery = donationQuery
                    .Where(d => d.Donator.FirstName.ToLower().Contains(donatorName.ToLower()) ||
                                d.Donator.LastName.ToLower().Contains(donatorName.ToLower()));
            }
            if (minAmount != null)
            {
                donationQuery = donationQuery.Where(d => d.Amount >= minAmount);
            }
            if (maxAmount != null)
            {
                donationQuery = donationQuery.Where(d => d.Amount < maxAmount);
            }
            if (dateAfter != null)
            {
                donationQuery = donationQuery.Where(d => d.Date >= dateAfter);
            }
            if (dateBefore != null)
            {
                donationQuery = donationQuery.Where(d => d.Date < dateBefore);
            }
            if (status != null)
            {
                donationQuery = donationQuery.Where(d => d.Status == status);
            }

            return await donationQuery.ToListAsync();
        }

        public async Task<Donation> TryAddDonatorAndShelterToDonation(Donation donationToCreate, string userId, int shelterId)
        {
            User? donator = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new NotFoundException(typeof(User).Name, userId);
            Shelter? shelter = await _context.Shelters
                .FirstOrDefaultAsync(s => s.Id == shelterId) ?? throw new NotFoundException(typeof(Shelter).Name, shelterId);

            donationToCreate.Donator = donator;
            donationToCreate.Shelter = shelter;

            return donationToCreate;
        }

        public async Task<Donation> UpdateDonationStatus(int id, DonationStatus newStatus)
        {
            Donation donationToUpdate = await GetAsync(id);

            if ((int)newStatus > Enum.GetValues(typeof(DonationStatus)).Length - 1 || (int)newStatus < 0)
                throw new BadRequestException("Invalid DonationStatus");

            donationToUpdate.Status = newStatus;
            await UpdateAsync(donationToUpdate);

            return donationToUpdate;
        }

        public async Task<PagedResult<TResult>> GetPagedAndFilteredDonationsAsync<TResult>(QueryParameters queryParameters, string? shelterName, string? donatorName, decimal? minAmount, decimal? maxAmount, DateTime? dateAfter, DateTime? dateBefore, DonationStatus? status)
        {
            List<Expression<Func<Donation, bool>>> filters = new();

            if (!string.IsNullOrWhiteSpace(shelterName))
            {
                Expression<Func<Donation, bool>> shelterNameExpression = d => d.Shelter.Name.ToLower().Contains(shelterName.ToLower());
                filters.Add(shelterNameExpression);
            }
            if (!string.IsNullOrWhiteSpace(donatorName))
            {
                Expression<Func<Donation, bool>> donatorNameExpression = d => d.Donator.FirstName.ToLower().Contains(donatorName.ToLower()) ||
                                                                         d.Donator.LastName.ToLower().Contains(donatorName.ToLower());
                filters.Add(donatorNameExpression);
            }
            if (minAmount != null)
            {
                Expression<Func<Donation, bool>> minAmountExpression = d => d.Amount >= minAmount;
                filters.Add(minAmountExpression);
            }
            if (maxAmount != null)
            {
                Expression<Func<Donation, bool>> maxAmountExpression = d => d.Amount < maxAmount;
                filters.Add(maxAmountExpression);
            }
            if (dateAfter != null)
            {
                Expression<Func<Donation, bool>> dateAfterExpression = d => d.Date >= dateAfter;
                filters.Add(dateAfterExpression);
            }
            if (dateBefore != null)
            {
                Expression<Func<Donation, bool>> dateBeforeExpression = d => d.Date < dateBefore;

            }
            if (status != null)
            {
                Expression<Func<Donation, bool>> statusExpression = d => d.Status == status;
                filters.Add(statusExpression);
            }

            return await GetPagedAndFiltered<TResult>(queryParameters, filters);
        }
    }
}
