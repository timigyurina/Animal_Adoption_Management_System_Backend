using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AnimalBreedService : GenericRepository<AnimalBreed>, IAnimalBreedService
    {
        public AnimalBreedService(AnimalAdoptionContext context, IMapper mapper) : base(context, mapper)
        {
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
                breedFilterQuery = breedFilterQuery.Where(b => b.Type == type);
            }

            return await breedFilterQuery.ToListAsync();
        }

        public async Task<PagedResult<TResult>> GetPagedAndFilteredBreedsAsync<TResult>(QueryParameters queryParameters, string? name, AnimalType? type)
        {
            List<Expression<Func<AnimalBreed, bool>>> filters = new();
            if (!string.IsNullOrWhiteSpace(name))
            {
                Expression<Func<AnimalBreed, bool>> namePredicate = b => b.Name.ToLower().Contains(name.ToLower());
                filters.Add(namePredicate);
            }
            if (type != null)
            {
                Expression<Func<AnimalBreed, bool>> typePredicate = b => b.Type == type;
                filters.Add(typePredicate);
            }

            return await GetPagedAndFiltered<TResult>(queryParameters, filters);
        }
    }
}
