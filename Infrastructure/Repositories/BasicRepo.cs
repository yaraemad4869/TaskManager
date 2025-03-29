using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using TaskManager.Core.Interfaces;
using TaskManager.Data;
namespace TaskManager.Infrastructure.Repositories
{
    public class BasicRepo<T> : IBasicRepo<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BasicRepo(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<List<T>>? GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T>? GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(entity => EF.Property<int>(entity, "Id") == id);
        }
        public async Task<List<T>> GetByNameAsync(string name)
        {
            return await _dbSet.Where(entity => EF.Property<string>(entity, "Name") == name).ToListAsync() ?? await _dbSet.Where(entity => EF.Property<string>(entity, "Title") == name).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T>? UpdateAsync(T entity)
        {
            var entity1 = await _dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == EF.Property<int>(entity, "Id"));
            if (entity1 == null)
            {
                return null;
            }
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T>? DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return entity;
        }
        public async Task<T>? DeleteAsync(T entity)
        {
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<List<T>>? DeleteAllAsync(List<T> entities)
        {
            if (entities != null)
            {
                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            return entities;
        }
    }
}