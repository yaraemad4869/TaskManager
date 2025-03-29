using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Models;
using TaskManager.Core.Interfaces;
using TaskManager.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _db;
        //private readonly DbSet<T> _dbSet;
        public CategoryRepo(AppDbContext db)
        {
            _db = db;
            //_dbSet = dbSet;
        }
        public async Task<List<Category>> GetCategories()
        {
            //IQueryable<T> query = _dbSet;

            //User?user = await _db.Users.FirstOrDefaultAsync(u => u.Email == AppUser.Ide);
            return await _db.Categories.ToListAsync();
        }
        public async Task<Category> GetCategoryById(int id)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.Id == id/* && c.UserId == userId*/);
        }
        public async Task<Category> GetCategoryByName(string name)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.Name == name/* && c.UserId == userId*/);
        }

    }
}
