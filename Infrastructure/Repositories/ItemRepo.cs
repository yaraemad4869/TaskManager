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
        public async Task<List<T>> GetByTitle(string title, int userId)
        {
            return await _dbSet.Where(entity => EF.Property<string>(entity, "Title").ToLower() == title.ToLower() && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        }

        public async Task<List<T>> GetWithCategoryId(int categoryId, int userId)
        {
            return await _dbSet.Where(entity => EF.Property<int>(entity, "CategoryId") == categoryId && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        }
        public async Task<List<Item>> GetWithCategoryName(string categoryName, int userId)
        {
            return await _db.Items.Where(i => i.category.Name == categoryName && i.UserId==userId).ToListAsync();
            //return await _dbSet.Where(entity => EF.Property<string>(entity, "CategoryName") == categoryName && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        }
        public async Task<List<T>> GetWithColorId(int colorId, int userId)
        {
            return await _dbSet.Where(entity => EF.Property<int>(entity, "ColorId") == colorId && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        }
        public async Task<List<Item>> GetWithColorName(string colorName, int userId)
        {
            return await _db.Items.Where(i => i.color.Name == colorName && i.UserId == userId).ToListAsync();
            //return await _dbSet.Where(entity => EF.Property<string>(entity, "ColorName") == colorName && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        }
        //public async Task<List<T>> GetByTitle(string title, int userId)
        //{
        //    return await _dbSet.Where(entity => EF.Property<string>(entity, "Title").ToLower() == title.ToLower() && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        //}

        //public async Task<List<T>> GetWithCategoryId(int categoryId,int userId)
        //{
        //    return await _dbSet.Where(entity => EF.Property<int>(entity, "CategoryId") == categoryId && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        //}
        //public async Task<List<T>> GetWithCategoryName(string categoryName, int userId)
        //{
        //    return await _dbSet.Where(entity => EF.Property<string>(entity, "CategoryName") == categoryName && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        //}
        //public async Task<List<T>> GetWithColorId(int colorId, int userId)
        //{
        //    return await _dbSet.Where(entity => EF.Property<int>(entity, "ColorId") == colorId && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        //}
        //public async Task<List<T>> GetWithColorName(string colorName, int userId)
        //{
        //    return await _dbSet.Where(entity => EF.Property<string>(entity, "ColorName") == colorName && EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        //}
        //public async Task<T> GetById(int id, int userId)
        //{
        //    return await _dbSet.FirstOrDefaultAsync(entity => EF.Property<int>(entity, "Id") == id && EF.Property<int>(entity, "UserId") == userId);
        //}
    }
}
