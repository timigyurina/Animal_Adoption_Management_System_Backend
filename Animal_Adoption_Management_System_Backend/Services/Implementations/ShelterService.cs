using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class ShelterService : GenericRepository<Shelter>, IShelterService
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
    }
}
