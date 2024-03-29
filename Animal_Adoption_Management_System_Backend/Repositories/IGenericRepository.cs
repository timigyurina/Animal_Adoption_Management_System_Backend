﻿using Animal_Adoption_Management_System_Backend.Models.Pagination;
using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend.Repositories
{
    public interface IGenericRepository<T> where T : class
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

        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters, string includeProperties = "");
        Task<PagedResult<TResult>> GetPagedAndFiltered<TResult>(
            QueryParameters queryParameters,
            IEnumerable<Expression<Func<T, bool>>> filters,
            string includeProperties = "");
    }
}
