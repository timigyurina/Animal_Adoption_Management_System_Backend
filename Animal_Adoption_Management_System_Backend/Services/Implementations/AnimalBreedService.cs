using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AnimalBreedService : GenericService<AnimalBreed>, IAnimalBreedService
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
                Expression<Func<AnimalBreed, bool>> nameExpression = b => b.Name.ToLower().Contains(name.ToLower());
                filters.Add(nameExpression);
            }
            if (type != null)
            {
                Expression<Func<AnimalBreed, bool>> typeExpression = b => b.Type == type;
                filters.Add(typeExpression);
            }

            return await GetPagedAndFiltered<TResult>(queryParameters, filters);
        }
    }
}
