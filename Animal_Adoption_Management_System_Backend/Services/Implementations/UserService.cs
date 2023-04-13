using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userManager.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetAsync(string id)
        {
            User user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(typeof(User).Name, id);
            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException($"{typeof(User).Name} email", email);
            return user;
        }

        public async Task<IEnumerable<User>> GetFilteredUsersAsync(
            string? name,
            string? email,
            bool? isActive,
            bool? isContactOfShelter,
            string? shelterName,
            DateTime? bornAfter,
            DateTime? bornBefore)
        {
            IQueryable<User> userQuery = _userManager.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                userQuery = userQuery.Where(u => u.FirstName.ToLower().Contains(name.ToLower()) || u.LastName.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                userQuery = userQuery.Where(u => u.Email.ToLower().Contains(email.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(shelterName))
            {
                userQuery = userQuery.Where(u => u.Shelter != null && u.Shelter.Name.Contains(shelterName.ToLower()));
            }
            if (isActive != null)
            {
                userQuery = userQuery.Where(u => u.IsActive == isActive);
            }
            if (isContactOfShelter != null)
            {
                userQuery = userQuery.Where(u => u.IsContactOfShelter == isContactOfShelter);
            }
            if (bornAfter != null)
            {
                userQuery = userQuery.Where(u => u.DateOfBirth >= bornAfter);
            }
            if (bornBefore != null)
            {
                userQuery = userQuery.Where(u => u.DateOfBirth < bornBefore);
            }

            return await userQuery.ToListAsync();
        }

        public async Task<User> GetWithAllDetailsAsync(string id)
        {
            CheckIfExists(id);

            User user = await _userManager.Users
                .Include(u => u.Shelter)
                .Include(u => u.Donations)
                .Include(u => u.Images)
                .Include(u => u.AdoptionApplications)
                .Include(u => u.AdoptionsContracts)
                .Include(u => u.ManagedAdoptionsContracts)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetWithShelterDetailsAsync(string id)
        {
            CheckIfExists(id);

            User user = await _userManager.Users
                .Include(u => u.Shelter)
                .AsNoTracking()
                .FirstAsync(u => u.Id == id);
            if (user.Shelter == null)
                throw new BadRequestException("User is not an employee of any Shelter");

            user = await _userManager.Users
                .Include(u => u.Shelter!)
                    .ThenInclude(s => s.Animals)
                        .ThenInclude(s => s.Animal)
                .Include(u => u.Shelter!)
                    .ThenInclude(s => s.Donations)
                .Include(u => u.Shelter!)
                    .ThenInclude(s => s.Employees)
                .Include(u => u.Shelter!)
                    .ThenInclude(s => s.Address)
                //.AsNoTracking()
                .FirstAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetWithDonationDetailsAsync(string id)
        {
            CheckIfExists(id);

            User user = await _userManager.Users
                .Include(u => u.Donations)
                    .ThenInclude(d => d.Shelter)
                .AsNoTracking()
                .FirstAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetWithImageDetailsAsync(string id)
        {
            CheckIfExists(id);

            User user = await _userManager.Users
                .Include(u => u.Images)
                    .ThenInclude(i => i.Animal)
                .AsNoTracking()
                .FirstAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetWithAdoptionApplicationDetailsAsync(string id)
        {
            CheckIfExists(id);

            User user = await _userManager.Users
                .Include(u => u.AdoptionApplications)
                    .ThenInclude(aa => aa.Animal)
                .AsNoTracking()
                .FirstAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetWithAdoptionContractDetailsAsync(string id)
        {
            CheckIfExists(id);

            User user = await _userManager.Users
                .Include(u => u.AdoptionsContracts)
                    .ThenInclude(ac => ac.Animal)
                .Include(u => u.AdoptionsContracts)
                    .ThenInclude(ac => ac.ApplierAddress)
                .AsNoTracking()
                .FirstAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetWithManagedAdoptionContractDetailsAsync(string id)
        {
            CheckIfExists(id);

            User user = await _userManager.Users
                .Include(u => u.ManagedAdoptionsContracts)
                    .ThenInclude(mac => mac.Contract)
                        .ThenInclude(c => c.Animal)
                .Include(u => u.ManagedAdoptionsContracts)
                    .ThenInclude(mac => mac.Contract)
                        .ThenInclude(c => c.Applier)
                .AsNoTracking()
                .FirstAsync(u => u.Id == id);

            return user;
        }


        public async Task CreateConnectionWithShelterByEmail(Shelter shelter, string email, bool isContactOfShelter)
        {
            User user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException(typeof(User).Name, email);
            user.IsContactOfShelter = isContactOfShelter;
            user.Shelter = shelter;

            await _userManager.UpdateAsync(user);
        }

        public async Task<User> UpdateUserIsActive(string id, bool isActive)
        {
            User user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(typeof(User).Name, id);
            user.IsActive = isActive;

            await _userManager.UpdateAsync(user);
            return user;
        }

        public async Task<User> UpdateConnectionWithShelterById(Shelter? shelter, string id, bool isContactOfShelter)
        {
            CheckIfExists(id);

            User user = await _userManager.Users
                .Include(u => u.Shelter)
                .FirstAsync(u => u.Id == id);

            user.IsContactOfShelter = isContactOfShelter;
            user.Shelter = shelter;

            await _userManager.UpdateAsync(user);
            return user;
        }

        private void CheckIfExists(string id)
        {
            bool userExists = _userManager.Users.Any(u => u.Id == id);
            if (!userExists)
                throw new NotFoundException(typeof(User).Name, id);
        }
    }
}
