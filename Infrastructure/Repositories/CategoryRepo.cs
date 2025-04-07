using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Models;
using TaskManager.Core.Interfaces;
using TaskManager.Data;
using AutoMapper;
using System.Security.Claims;

namespace TaskManager.Infrastructure.Repositories
{
    public class CategoryRepo<T> : ICategoryRepo<T> where T : class
    {
        private readonly AppDbContext _db;
        private DbSet<T> _dbSet;
        public CategoryRepo(AppDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }
        public async Task<List<T>> GetAllByUserId(int userId)
        {
            //IQueryable<T> query = _dbSet;

            //User?user = await _db.Users.FirstOrDefaultAsync(u => u.Email == AppUser.Ide);
            return await _dbSet.Where(entity => EF.Property<int>(entity, "UserId") == userId).ToListAsync();
        }
        public async Task<bool> IsUnique(string name, int userId)
        {
            return await _dbSet.AnyAsync(entity => EF.Property<int>(entity, "UserId") == userId && EF.Property<string>(entity, "Name").ToLower() == name.ToLower());

        }
        public async Task<T> GetOneByIdAndUserId(int id, int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(entity => EF.Property<int>(entity, "UserId") == userId && EF.Property<int>(entity, "Id") == id);
        }
        public async Task<List<T>> GetOneByNameAndUserId(string name, int userId)
        {
            return await _dbSet.Where(entity => EF.Property<int>(entity, "UserId") == userId && EF.Property<string>(entity, "Name").ToLower().Contains(name.ToLower())).ToListAsync();
        }
        public async Task<T>? DeleteAsync(int id, int userId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(entity => EF.Property<int>(entity, "UserId") == userId && EF.Property<int>(entity, "Id") == id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _db.SaveChangesAsync();
            }
            return entity;
        }
        public async Task<T>? DeleteAsync(string name, int userId)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(entity => EF.Property<int>(entity, "UserId") == userId && EF.Property<string>(entity, "Name").ToLower() == name.ToLower());
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _db.SaveChangesAsync();
            }
            return entity;
        }
    }
}
