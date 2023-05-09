using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AnimalService : GenericRepository<Animal>, IAnimalService
    {
        public AnimalService(AnimalAdoptionContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<Animal>> GetFilteredAnimalsAsync(
            string? name,
            AnimalType? type,
            AnimalSize? size,
            AnimalStatus? status,
            Gender? gender,
            AnimalColor? color,
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
            if (type != null)
            {
                animalQuery = animalQuery.Where(a => a.Type == type);
            }
            if (size != null)
            {
                animalQuery = animalQuery.Where(a => a.Size == size);
            }
            if (status != null)
            {
                animalQuery = animalQuery.Where(a => a.Status == status);
            }
            if (gender != null)
            {
                animalQuery = animalQuery.Where(a => a.Gender == gender);
            }
            if (color != null)
            {
                animalQuery = animalQuery.Where(a => a.Color == color);
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

        public async Task<Animal> GetWithImagesAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(Animal).Name, id);

            return await _context.Animals
                .Include(a => a.Images)
                .AsNoTracking()
                .FirstAsync(a => a.Id == id);
        }

        public async Task<Animal> GetWithAnimalShelterDetailsAsync(int animalId)
        {
            if (!await Exists(animalId))
                throw new NotFoundException(typeof(Animal).Name, animalId);

            return await _context.Animals
                .Include(a => a.AnimalShelters)
                    .ThenInclude(s => s.Shelter)
                .FirstAsync(a => a.Id == animalId);
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
