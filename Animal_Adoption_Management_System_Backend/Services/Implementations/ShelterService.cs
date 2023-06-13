using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class ShelterService : GenericService<Shelter>, IShelterService
    {
        public ShelterService(AnimalAdoptionContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<Shelter>> GetFilteredSheltersAsync(string? name, string? contactPersonName, bool? isActive)
        {
            IQueryable<Shelter> shelterQuery = _context.Shelters
                .Include(s => s.Employees)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                shelterQuery = shelterQuery.Where(s => s.Name.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(contactPersonName))
            {
                shelterQuery = shelterQuery
                    .Where(s => s.Employees
                        .Any(e => e.Shelter != null && e.FirstName.ToLower().Contains(contactPersonName.ToLower()) ||
                                                       e.LastName.ToLower().Contains(contactPersonName.ToLower())));
            }
            if (isActive != null)
            {
                shelterQuery = shelterQuery.Where(s => s.IsActive == isActive);
            }

            return await shelterQuery.ToListAsync();
        }

        public async Task<Shelter> GetWithAddressAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(Shelter).Name, id);

            return await _context.Shelters
                .Include(s => s.Address)
                .FirstAsync(s => s.Id == id);
        }

        public async Task<Shelter> GetWithAddressAndAnimalsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(Shelter).Name, id);

            return await _context.Shelters
                .Include(s => s.Address)
                .Include(s => s.Animals)
                    .ThenInclude(a => a.Animal)
                        .ThenInclude(a => a.Breed)
                .FirstAsync(s => s.Id == id);
        }

        public async Task<Shelter> GetWithDonationsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(Shelter).Name, id);

            return await _context.Shelters
                .Include(s => s.Donations)
                    .ThenInclude(d => d.Donator)
                .AsNoTracking()
                .FirstAsync(s => s.Id == id);
        }

        public async Task<Shelter> GetWithDetailsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(Shelter).Name, id);

            return await _context.Shelters
                .Include(s => s.Address)
                .Include(s => s.Employees)
                .Include(s => s.Donations)
                .Include(s => s.Animals)
                    .ThenInclude(a => a.Animal)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstAsync(s => s.Id == id);
        }

        public async Task<Shelter> UpdateShelterIsActive(int id, bool isActive)
        {
            Shelter shelterToUpdate = await GetAsync(id);

            shelterToUpdate.IsActive = isActive;
            await UpdateAsync(shelterToUpdate);

            return shelterToUpdate;
        }

        public async Task<Shelter> UpdateShelterContactPerson(int id, string contactPersonId)
        {
            Shelter shelterToUpdate = await GetAsync(id);
            User? contactPerson = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == contactPersonId) ?? throw new NotFoundException(typeof(User).Name, contactPersonId);

            await UpdateAsync(shelterToUpdate);

            return shelterToUpdate;
        }

        public async Task<PagedResult<TResult>> GetPagedAndFilteredSheltersAsync<TResult>(
            QueryParameters queryParameters,
            string? name, string? contactPersonName, bool? isActive)
        {
            List<Expression<Func<Shelter, bool>>> filters = new();
            if (!string.IsNullOrWhiteSpace(name))
            {
                Expression<Func<Shelter, bool>> nameExpression = s => s.Name.ToLower().Contains(name.ToLower());
                filters.Add(nameExpression);
            }
            if (!string.IsNullOrWhiteSpace(contactPersonName))
            {
                Expression<Func<Shelter, bool>> contactPersonNameExpression = s => s.Employees
                        .Any(e => e.IsContactOfShelter && (e.FirstName.ToLower().Contains(contactPersonName.ToLower()) ||
                                                       e.LastName.ToLower().Contains(contactPersonName.ToLower())));
                filters.Add(contactPersonNameExpression);
            }
            if (isActive != null)
            {
                Expression<Func<Shelter, bool>> isActiveExpression = s => s.IsActive == isActive;
                filters.Add(isActiveExpression);
            }

            return await GetPagedAndFiltered<TResult>(queryParameters, filters);

        }
    }
}
