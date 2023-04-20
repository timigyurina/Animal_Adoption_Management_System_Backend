using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class ManagedAdoptionContractService : GenericRepository<ManagedAdoptionContract>, IManagedAdoptionContractService
    {
        public ManagedAdoptionContractService(AnimalAdoptionContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<ManagedAdoptionContract> GetByAdoptionContractIdAsync(int adoptionContractId)
        {
            if (!_context.ManagedAdoptionContracts.Any(m => m.Contract.Id == adoptionContractId))
                throw new NotFoundException(typeof(AdoptionContract).Name, adoptionContractId);

            ManagedAdoptionContract managedAdoptionContract = await _context.ManagedAdoptionContracts
                .Include(m => m.Manager)
                .AsNoTracking()
                .FirstAsync(m => m.Contract.Id == adoptionContractId);

            return managedAdoptionContract;
        }

        public async Task<ManagedAdoptionContract> TryAddRelatedEntitiesToManagedContract(string managerId, AdoptionContract adoptionContract)
        {
            User manager = await _context.Users
                .FirstOrDefaultAsync(a => a.Id == managerId) ?? throw new NotFoundException($"{typeof(User).Name} (manager)", managerId);

            ManagedAdoptionContract managedAdoptionContract = new()
            {
                Contract = adoptionContract,
                Manager = manager
            };

            return managedAdoptionContract;
        }
    }
}
