using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TaskManager.Data.DTOs;
using TaskManager.Data.Models;

using TaskManager.Data.Context;
using TaskManager.IRepositories;

namespace TaskManager.Repositories
{
    public class UserRepo :IUserRepo
    {
        private readonly AppDbContext _db;
        public UserRepo(AppDbContext db) {
            _db = db;
        }
        public async Task<List<User>> GetUserByNameAsync(string name, params Func<IQueryable<User>?, IIncludableQueryable<User, object>>[]? includes)
        {
            return await _db.Users.Where(u => u.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
        public async Task<User> GetUserByEmailAsync(string email, params Func<IQueryable<User>?, IIncludableQueryable<User, object>>[]? includes)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Trim()==email.ToLower().Trim());

        }
        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber, params Func<IQueryable<User>?, IIncludableQueryable<User, object>>[]? includes)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Trim() == phoneNumber.Trim());
        }

        public async Task<User> VerifyUser(LoginDTO user, params Func<IQueryable<User>?, IIncludableQueryable<User, object>>[]? includes)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
        }
    }
}
