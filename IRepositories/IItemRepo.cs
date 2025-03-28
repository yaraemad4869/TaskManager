using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Models;

namespace TaskManager.IRepositories
{
    public interface IItemRepo<T> where T : class
    {
        Task<List<T>> GetWithCategory(User user, int categoryId);
        Task<T> GetById(User user, int id);
    }
}
