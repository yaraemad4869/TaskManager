using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace TaskManager.Core.Interfaces
{
    public interface IBasicRepo<T> where T : class
    {
        Task<List<T>>? GetAllAsync();
        Task<T>? GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T>? UpdateAsync(T entity);
        Task<T>? DeleteAsync(int id);
        Task<T>? DeleteAsync(T entity);
        Task<List<T>>? DeleteAllAsync(List<T> entities);
    }
}
