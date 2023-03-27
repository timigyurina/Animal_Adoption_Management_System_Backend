using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal readonly AnimalAdoptionContext _context;
        internal readonly DbSet<T> _dbSet;
        public GenericRepository(AnimalAdoptionContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
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
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
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
    }
}
