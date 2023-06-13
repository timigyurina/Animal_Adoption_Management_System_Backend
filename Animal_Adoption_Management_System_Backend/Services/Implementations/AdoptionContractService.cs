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
    public class AdoptionContractService : GenericService<AdoptionContract>, IAdoptionContractService
    {
        public AdoptionContractService(AnimalAdoptionContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<AdoptionContract> GetWithDetailsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(AdoptionContract).Name, id);
            return await _context.AdoptionContracts
                .Include(a => a.Animal)
                .Include(a => a.Applier)
                .AsNoTracking()
                .FirstAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<AdoptionContract>> GetFilteredAdoptionContractsAsync(
            string? animalName,
            string? applierName,
            DateTime? dateAfter,
            DateTime? dateBefore,
            bool? isActive)
        {
            IQueryable<AdoptionContract> adoptionContractQuery = _context.AdoptionContracts.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(animalName))
            {
                adoptionContractQuery = adoptionContractQuery.Where(a => a.Animal.Name.ToLower().Contains(animalName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(applierName))
            {
                adoptionContractQuery = adoptionContractQuery
                    .Where(a => a.Applier.LastName.ToLower().Contains(applierName.ToLower()) || a.Applier.FirstName.ToLower().Contains(applierName.ToLower()));
            }
            if (dateAfter != null)
            {
                adoptionContractQuery = adoptionContractQuery.Where(a => a.ContractDate >= dateAfter);
            }
            if (dateBefore != null)
            {
                adoptionContractQuery = adoptionContractQuery.Where(a => a.ContractDate < dateBefore);
            }
            if (isActive != null)
            {
                adoptionContractQuery = adoptionContractQuery.Where(a => a.IsActive == isActive);
            }

            return await adoptionContractQuery.ToListAsync();
        }

        public async Task<AdoptionContract> TryAddRelatedEntitiesToAdoptionContract(AdoptionContract adoptionContractToCreate, int animalId, string applierId)
        {
            Animal animal = await _context.Animals
                .Include(a => a.AnimalShelters)
                    .ThenInclude(s => s.Shelter)
                .FirstOrDefaultAsync(a => a.Id == animalId) ?? throw new NotFoundException(typeof(Animal).Name, animalId);

            if (animal.Status != AnimalStatus.WaitingForAdoption)
                throw new BadRequestException($"Animal with id {animalId} is not adoptable");

            User applier = await _context.Users
                .FirstOrDefaultAsync(a => a.Id == applierId) ?? throw new NotFoundException($"{typeof(User).Name} (applier)", applierId);

            adoptionContractToCreate.Animal = animal;
            adoptionContractToCreate.Applier = applier;
            adoptionContractToCreate.IsActive = true;

            return adoptionContractToCreate;
        }

        public async Task<AdoptionContract> UpdateAdoptionContractIsActive(int id, bool isActive)
        {
            AdoptionContract adoptionContract = await GetAsync(id);

            adoptionContract.IsActive = isActive;
            await UpdateAsync(adoptionContract);

            return adoptionContract;
        }

        public async Task<AdoptionContract> GetWithAnimalShelterDetailsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(AdoptionContract).Name, id);
            return await _context.AdoptionContracts
                .Include(a => a.Animal)
                    .ThenInclude(animal => animal.AnimalShelters)
                        .ThenInclude(animalS => animalS.Shelter)
                .Include(a => a.Applier)
                .AsNoTracking()
                .FirstAsync(a => a.Id == id);
        }

        public async Task<PagedResult<TResult>> GetPagedAndFilteredAdoptionContractsAsync<TResult>(QueryParameters queryParameters, string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, bool? isActive)
        {
            List<Expression<Func<AdoptionContract, bool>>> filters = new();
            if (!string.IsNullOrWhiteSpace(animalName))
            {
                Expression<Func<AdoptionContract, bool>> animalNameExpression = a => a.Animal.Name.ToLower().Contains(animalName.ToLower());
                filters.Add(animalNameExpression);
            }
            if (!string.IsNullOrWhiteSpace(applierName))
            {
                Expression<Func<AdoptionContract, bool>> applierNameExpression = a => a.Applier.LastName.ToLower().Contains(applierName.ToLower()) || a.Applier.FirstName.ToLower().Contains(applierName.ToLower());
                filters.Add(applierNameExpression);
            }
            if (dateAfter != null)
            {
                Expression<Func<AdoptionContract, bool>> dateAfterExpression = a => a.ContractDate >= dateAfter;
                filters.Add(dateAfterExpression);
            }
            if (dateBefore != null)
            {
                Expression<Func<AdoptionContract, bool>> dateBeforeExpression = a => a.ContractDate < dateBefore;
                filters.Add(dateBeforeExpression);
            }
            if (isActive != null)
            {
                Expression<Func<AdoptionContract, bool>> isActiveExpression = a => a.IsActive == isActive;
                filters.Add(isActiveExpression);
            }

            return await GetPagedAndFiltered<TResult>(queryParameters, filters);
        }
    }
}
