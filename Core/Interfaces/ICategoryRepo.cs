using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models;

namespace TaskManager.Core.Interfaces
{
    public interface ICategoryRepo
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> GetCategoryByName(string name);
    }
}
