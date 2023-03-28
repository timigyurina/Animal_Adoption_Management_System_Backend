using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AnimalService : GenericRepository<Animal>, IAnimalService
    {
        public AnimalService(AnimalAdoptionContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Animal>> GetFilteredAnimalsAsync(
            string? name,
            string? type,
            string? size,
            string? status,
            string? gender,
            string? color,
            int? breedId,
            bool? isSterilised,
            DateTime? bornAfter,
            DateTime? bornBefore)
        {
            IQueryable<Animal> animalQuery = _context.Animals.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                animalQuery = animalQuery
                    .Where(a => a.Name.ToLower().Contains(name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                bool animalTypeParsed = int.TryParse(type, out int animalTypeNumber);
                if (!animalTypeParsed || animalTypeNumber >= Enum.GetNames(typeof(AnimalType)).Length || animalTypeNumber < 0)
                    throw new BadRequestException($"Incorrect {nameof(AnimalType)}");

                animalQuery = animalQuery.Where(a => (int)a.Type == animalTypeNumber);
            }
            if (!string.IsNullOrWhiteSpace(size))
            {
                bool animalSizeParsed = int.TryParse(size, out int animalSizeNumber);
                if (!animalSizeParsed || animalSizeNumber >= Enum.GetNames(typeof(AnimalSize)).Length || animalSizeNumber < 0)
                    throw new BadRequestException($"Incorrect {nameof(AnimalSize)}");

                animalQuery = animalQuery.Where(a => (int)a.Size == animalSizeNumber);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                bool animalStatusParsed = int.TryParse(status, out int animalStatusNumber);
                if (!animalStatusParsed || animalStatusNumber >= Enum.GetNames(typeof(AnimalStatus)).Length || animalStatusNumber < 0)
                    throw new BadRequestException($"Incorrect {nameof(AnimalStatus)}");

                animalQuery = animalQuery.Where(a => (int)a.Status == animalStatusNumber);
            }
            if (!string.IsNullOrWhiteSpace(gender))
            {
                bool animalGenderParsed = int.TryParse(gender, out int animalGenderNumber);
                if (!animalGenderParsed || animalGenderNumber >= Enum.GetNames(typeof(Gender)).Length || animalGenderNumber < 0)
                    throw new BadRequestException($"Incorrect {nameof(Gender)}");

                animalQuery = animalQuery.Where(a => (int)a.Gender == animalGenderNumber);
            }
            if (!string.IsNullOrWhiteSpace(color))
            {
                bool animalColorParsed = int.TryParse(color, out int animalColorNumber);
                if (!animalColorParsed || animalColorNumber >= Enum.GetNames(typeof(AnimalColor)).Length || animalColorNumber < 0)
                    throw new BadRequestException($"Incorrect {nameof(AnimalColor)}");

                animalQuery = animalQuery.Where(a => (int)a.Color == animalColorNumber);
            }
            if (breedId != null)
            {
                animalQuery = animalQuery.Where(a => a.Breed.Id == breedId);
            }
            if (isSterilised != null)
            {
                animalQuery = animalQuery.Where(a => a.IsSterilised == isSterilised);
            }
            if (bornAfter != null)
            {
                animalQuery = animalQuery.Where(a => a.BirthDate >= bornAfter);
            }
            if (bornBefore != null)
            {
                animalQuery = animalQuery.Where(a => a.BirthDate < bornBefore);
            }

            return await animalQuery.ToListAsync();
        }


        public async Task<Animal> GetWithDetailsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(Animal).Name, id);

            return await _context.Animals
                .Include(a => a.Breed)
                .Include(a => a.AnimalShelters)
                    .ThenInclude(s => s.Shelter)
                .Include(a => a.AdoptionApplications)
                .Include(a => a.AdoptionContracts)
                .Include(a => a.Images)
                .AsNoTracking()
                .FirstAsync(a => a.Id == id);
        }

        public override Task<Animal> AddAsync(Animal entity)
        {
            CheckAnimalAndAnimalTypeBreedMatch(entity);
            return base.AddAsync(entity);
        }

        public override Task UpdateAsync(Animal entity)
        {
            if (entity.Breed != null)
                CheckAnimalAndAnimalTypeBreedMatch(entity);
            return base.UpdateAsync(entity);
        }

        public async Task<Animal> UpdateSterilisation(int id, UpdateSterilisationDTO sterilisationDate)
        {
            Animal animal = await GetAsync(id);

            animal.IsSterilised = true;
            animal.SterilisationDate = sterilisationDate.SterilisationDate;
            await UpdateAsync(animal);

            return animal;
        }

        public async Task<Animal> UpdateStatus(int id, AnimalStatus newStatus)
        {
            Animal animal = await GetAsync(id);
            if ((int)newStatus > Enum.GetValues(typeof(AnimalStatus)).Length - 1 || (int)newStatus < 0)
                throw new BadRequestException("Invalid AnimalStatus");

            animal.Status = newStatus;
            await UpdateAsync(animal);

            return animal;
        }

        private void CheckAnimalAndAnimalTypeBreedMatch(Animal entity)
        {
            if (entity.Type != entity.Breed.Type)
                throw new BadRequestException("The type of the Animal and the type of the provided Breed do not match");
        }
    }
}
