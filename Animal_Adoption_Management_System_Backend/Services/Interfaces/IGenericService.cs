using Animal_Adoption_Management_System_Backend.Models.Pagination;
using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = ""
        );
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> Exists(int id);

        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
        Task<PagedResult<TResult>> GetPagedAndFiltered<TResult>(
            QueryParameters queryParameters,
            IEnumerable<Expression<Func<T, bool>>> filters);
    }
}
