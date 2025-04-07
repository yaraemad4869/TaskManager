using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models;

namespace TaskManager.Core.Interfaces
{
    public interface ICategoryRepo<T> where T : class
    {
        Task<List<T>> GetAllByUserId(int userId);
        Task<bool> IsUnique(string name, int userId);
        Task<T> GetOneByIdAndUserId(int id, int userId);
        Task<List<T>> GetOneByNameAndUserId(string name, int userId);
        Task<T>? DeleteAsync(int id, int userId);
        Task<T>? DeleteAsync(string name, int userId);
    }
}
