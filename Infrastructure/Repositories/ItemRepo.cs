using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using TaskManager.Core.Models;
using TaskManager.Core.Interfaces;
using TaskManager.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ItemRepo<T> : IItemRepo<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _dbSet;

        public ItemRepo(AppDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();

        }

        public async Task<List<T>> GetWithCategory(User user, int categoryId = 0)
        {
            return await _dbSet.Where(entity => EF.Property<int>(entity, "CategoryId") == categoryId && EF.Property<int>(entity, "UserId") == user.Id).ToListAsync();
        }
        public async Task<T> GetById(User user, int id)
        {
            return await _dbSet.FirstOrDefaultAsync(entity => EF.Property<int>(entity, "Id") == id && EF.Property<int>(entity, "UserId") == user.Id);
        }
    }
}
