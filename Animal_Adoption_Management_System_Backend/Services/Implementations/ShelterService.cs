﻿using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class ShelterService : GenericRepository<Shelter>, IShelterService
    {
        public ShelterService(AnimalAdoptionContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Shelter>> GetFilteredSheltersAsync(string? name, string? contactPersonName, bool? isActive)
        {
            IQueryable<Shelter> shelterQuery = _context.Shelters
                .Include(s => s.ContactPerson)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                shelterQuery = shelterQuery.Where(s => s.Name.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(contactPersonName))
            {
                shelterQuery = shelterQuery
                    .Where(s => s.ContactPerson!.FirstName.ToLower().Contains(contactPersonName.ToLower()) ||
                                s.ContactPerson.LastName.ToLower().Contains(contactPersonName.ToLower()));
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
                .Include(s => s.ContactPerson)
                .Include(s => s.Donations)
                .Include(s => s.Animals)
                    .ThenInclude(a => a.Animal)
                .AsNoTracking()
                .FirstAsync(s => s.Id == id);
        }

        public async Task<Shelter> TryAddContactPersonToShelter(Shelter shelterToCreate, string? contactPersonId)
        {
            User? contactPerson;
            if (contactPersonId != null)
                contactPerson = await _context.Users.FirstOrDefaultAsync(u => u.Id == contactPersonId) ?? throw new NotFoundException(typeof(User).Name, contactPersonId);
            else
                contactPerson = null;

            shelterToCreate.ContactPerson = contactPerson;
            return shelterToCreate;
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

            shelterToUpdate.ContactPerson = contactPerson;
            await UpdateAsync(shelterToUpdate);

            return shelterToUpdate;
        }
    }
}
