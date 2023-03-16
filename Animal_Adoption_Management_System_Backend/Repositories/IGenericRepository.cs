using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(int? id);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = ""
        );
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> Exists(int id);
    }
}
