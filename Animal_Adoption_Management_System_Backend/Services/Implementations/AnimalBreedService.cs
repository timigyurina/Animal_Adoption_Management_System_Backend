using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AnimalBreedService : GenericRepository<AnimalBreed>, IAnimalBreedService
    {
        public AnimalBreedService(AnimalAdoptionContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AnimalBreed>> GetFilteredBreedsAsync1(string? name, string? type)
        {
            IEnumerable<AnimalBreed> allBreeds = await GetAllAsync();

            if (!string.IsNullOrWhiteSpace(name))
            {
                allBreeds = allBreeds
                    .Where(b => b.Name.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                allBreeds = allBreeds
                    .Where(b => b.Type.ToString().ToLower() == type.ToLower());
            }

            return allBreeds;
        }
        public async Task<IEnumerable<AnimalBreed>> GetFilteredBreedsAsync(string? name, string? type)
        {
            IQueryable<AnimalBreed> breedFilterQuery = _context.AnimalBreeds.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                breedFilterQuery = breedFilterQuery
                    .Where(b => b.Name.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                var breedTypeParsed = int.TryParse(type, out int breedTypeNumber);
                if (!breedTypeParsed || breedTypeNumber >= Enum.GetNames(typeof(AnimalType)).Length || breedTypeNumber < 0)
                    throw new BadRequestException("Incorrect AnimalType");

                breedFilterQuery = breedFilterQuery.Where(b => (int)b.Type == breedTypeNumber);
            }

            return await breedFilterQuery.ToListAsync();
        }
        public async Task<IEnumerable<AnimalBreed>> GetFilteredBreedsAsync(string? name, AnimalType? type)
        {
            IQueryable<AnimalBreed> breedFilterQuery = _context.AnimalBreeds.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                breedFilterQuery = breedFilterQuery
                    .Where(b => b.Name.ToLower().Contains(name.ToLower()));
            }
            if (type != null)
            {
                if ((int)type > Enum.GetValues(typeof(AnimalType)).Length - 1 || (int)type < 0)
                    throw new BadRequestException("Invalid AnimalType");

                breedFilterQuery = breedFilterQuery.Where(b => b.Type == type);
            }

            return await breedFilterQuery.ToListAsync();
        }
    }
}
