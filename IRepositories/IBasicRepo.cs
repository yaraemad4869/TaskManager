using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace TaskManager.IRepositories
{
    public interface IBasicRepo<T> where T : class 
    {
        Task<List<T>>? GetAllAsync();
        Task<List<T>>? GetAllAsync(params Func<IQueryable<T>?, IIncludableQueryable<T, object>>[]? includes);
        Task<T>? GetByIdAsync(int id, params Func<IQueryable<T>?, IIncludableQueryable<T, object>>[]? includes);
        Task AddAsync(T entity);
        Task<T>? UpdateAsync(T entity);
        Task<T>? DeleteAsync(int id);
        Task<T>? DeleteAsync(T entity);
        Task<List<T>>? DeleteAllAsync(List<T> entities);
    }
}
