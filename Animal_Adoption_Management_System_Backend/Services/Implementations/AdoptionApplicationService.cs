using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AdoptionApplicationService : GenericService<AdoptionApplication>, IAdoptionApplicationService
    {
        public AdoptionApplicationService(AnimalAdoptionContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<AdoptionApplication> GetWithDetailsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(AdoptionApplication).Name, id);

            return await _context.AdoptionApplications
                .Include(a => a.Animal)
                .Include(a => a.Applier)
                .AsNoTracking()
                .FirstAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<AdoptionApplication>> GetFilteredAdoptionApplicationsAsync(
            string? animalName,
            string? applierName,
            DateTime? dateAfter,
            DateTime? dateBefore,
            ApplicationStatus? status)
        {
            IQueryable<AdoptionApplication> adoptionApplicationQuery = _context.AdoptionApplications
                .Include(a => a.Animal)
                .Include(a => a.Applier)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(animalName))
            {
                adoptionApplicationQuery = adoptionApplicationQuery.Where(a => a.Animal.Name.ToLower().Contains(animalName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(applierName))
            {
                adoptionApplicationQuery = adoptionApplicationQuery
                    .Where(a => a.Applier.LastName.ToLower().Contains(applierName.ToLower()) || a.Applier.FirstName.ToLower().Contains(applierName.ToLower()));
            }
            if (dateAfter != null)
            {
                adoptionApplicationQuery = adoptionApplicationQuery.Where(a => a.ApplicationDate >= dateAfter);
            }
            if (dateBefore != null)
            {
                adoptionApplicationQuery = adoptionApplicationQuery.Where(a => a.ApplicationDate < dateBefore);
            }
            if (status != null)
            {
                adoptionApplicationQuery = adoptionApplicationQuery.Where(a => a.Status == status);
            }

            return await adoptionApplicationQuery.ToListAsync();
        }

        public async Task<AdoptionApplication> TryAddAnimalAndApplierToAdoptionApplication(int animalId, ClaimsPrincipal applier)
        {
            Animal animal = await _context.Animals
                .FirstOrDefaultAsync(a => a.Id == animalId) ?? throw new NotFoundException(typeof(Animal).Name, animalId);

            Claim? UserIdClaim = applier.Claims.FirstOrDefault(c => c.Type == "UserId") ?? throw new BadRequestException("Cannot add Applier to AdoptionApplication, no User was found");
            User foundApplier = await _context.Users
                .FirstOrDefaultAsync(a => a.Id == UserIdClaim.Value) ?? throw new NotFoundException(typeof(User).Name, UserIdClaim.Value);

            if (animal.Status != AnimalStatus.WaitingForAdoption)
                throw new BadRequestException($"Animal with id {animalId} is not adoptable");

            AdoptionApplication adoptionApplicationToCreate = new()
            {
                Animal = animal,
                Applier = foundApplier,
                ApplicationDate = DateTime.Today,
                Status = ApplicationStatus.Submitted
            };

            return adoptionApplicationToCreate;
        }

        public async Task<AdoptionApplication> UpdateAdoptionApplicationStatus(int id, ApplicationStatus newStatus)
        {
            AdoptionApplication adoptionApplicationToUpdate = await GetAsync(id);

            if ((int)newStatus > Enum.GetValues(typeof(ApplicationStatus)).Length - 1 || (int)newStatus < 0)
                throw new BadRequestException("Invalid ApplicationStatus");

            adoptionApplicationToUpdate.Status = newStatus;
            await UpdateAsync(adoptionApplicationToUpdate);

            return adoptionApplicationToUpdate;
        }

        public async Task SetAdoptionApplicationStatusForContractCreation(Animal animal, User applier)
        {
            AdoptionApplication adoptionApplication = await _context.AdoptionApplications
                .Where(a => a.Animal == animal && a.Applier == applier && a.Status == ApplicationStatus.Submitted).FirstOrDefaultAsync()
                ?? throw new BadRequestException($"AdoptionApplication for Animal {animal.Id} by User {applier.Id} has not been submitted");

            adoptionApplication.Status = ApplicationStatus.Approved;
            await UpdateAsync(adoptionApplication);
        }

        public override Task<AdoptionApplication> AddAsync(AdoptionApplication entity)
        {
            if (_context.AdoptionApplications.Any(a => a.Animal == entity.Animal && a.Applier == entity.Applier && a.Status == ApplicationStatus.Submitted))
                throw new BadRequestException("Adoption Application for this User and Animal has been already submitted");
            return base.AddAsync(entity);
        }

        public async Task<AdoptionApplication> GetWithAnimalShelterDetailsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(AdoptionApplication).Name, id);

            return await _context.AdoptionApplications
            .Include(a => a.Animal)
                .ThenInclude(animal => animal.AnimalShelters)
                    .ThenInclude(animalS => animalS.Shelter)
            .Include(a => a.Applier)
            .AsNoTracking()
            .FirstAsync(a => a.Id == id);
        }

        public async Task<PagedResult<TResult>> GetPagedAndFilteredAdoptionApplicationsAsync<TResult>(QueryParameters queryParameters, string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, ApplicationStatus? status)
        {
            List<Expression<Func<AdoptionApplication, bool>>> filters = new();

            if (!string.IsNullOrWhiteSpace(animalName))
            {
                Expression<Func<AdoptionApplication, bool>> animalNameExpression = a => a.Animal.Name.ToLower().Contains(animalName.ToLower());
                filters.Add(animalNameExpression);
            }
            if (!string.IsNullOrWhiteSpace(applierName))
            {
                Expression<Func<AdoptionApplication, bool>> applierNameExpression = a => a.Applier.LastName.ToLower().Contains(applierName.ToLower()) || a.Applier.FirstName.ToLower().Contains(applierName.ToLower());
                filters.Add(applierNameExpression);
            }
            if (dateAfter != null)
            {
                Expression<Func<AdoptionApplication, bool>> dateAfterExpression = a => a.ApplicationDate >= dateAfter;
                filters.Add(dateAfterExpression);
            }
            if (dateBefore != null)
            {
                Expression<Func<AdoptionApplication, bool>> dateBeforeExpression = a => a.ApplicationDate < dateBefore;
                filters.Add(dateBeforeExpression);
            }
            if (status != null)
            {
                Expression<Func<AdoptionApplication, bool>> statusExpression = a => a.Status == status;
                filters.Add(statusExpression);
            }
            return await GetPagedAndFiltered<TResult>(queryParameters, filters);
        }
    }
}
