using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Animal_Adoption_Management_System_Backend.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal readonly AnimalAdoptionContext _context;
        internal readonly DbSet<T> _dbSet;
        private readonly IMapper _mapper;

        public GenericRepository(AnimalAdoptionContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _mapper = mapper;
        }


        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            else
                return await query.ToListAsync();
        }


        public async Task<T> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id) ?? throw new NotFoundException(typeof(T).Name, id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            EntityEntry<T> entry = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T entityToDelete = await GetAsync(id);

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
                _dbSet.Attach(entityToDelete);

            _dbSet.Remove(entityToDelete);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> Exists(int id)
        {
            T? entity = await _dbSet.FindAsync(id);
            return entity != null;
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters, string includeProperties = "")
        {
            int totalSize = await _context.Set<T>().CountAsync();

            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            List<TResult> items = await query
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider) // TResult represents the DTO, T represents the model
                .ToListAsync();

            return new PagedResult<TResult>
            {
                Items = items,
                CurrentPage = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                TotalCount = totalSize
            };
        }
        
        public async Task<PagedResult<TResult>> GetPagedAndFiltered<TResult>(
            QueryParameters queryParameters, 
            IEnumerable<Expression<Func<T, bool>>> filters,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (filters.Any())
            {
                foreach (var filter in filters)
                    query = query.Where(filter);
            }

            List<TResult> items = await query
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider) 
                .ToListAsync();

            return new PagedResult<TResult>
            {
                Items = items,
                CurrentPage = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                TotalCount = query.ToList().Count
            };
        }
    }
}
