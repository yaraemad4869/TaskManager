using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Models;

namespace TaskManager.Core.Interfaces
{
    public interface IItemRepo<T> where T : class
    {
        Task<List<T>> GetByTitle(string title, int userId);
        Task<List<T>> GetWithCategoryId(int categoryId, int userId);
        Task<List<Item>> GetWithCategoryName(string categoryName, int userId);
        Task<List<T>> GetWithColorId(int colorId, int userId);
        Task<List<Item>> GetWithColorName(string colorName, int userId);
    }
}
