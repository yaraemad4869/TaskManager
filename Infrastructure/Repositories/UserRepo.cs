using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TaskManager.Core.Models;
using TaskManager.DTOs;
using TaskManager.Core.Interfaces;
using TaskManager.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _db;
        public UserRepo(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<User>> GetUserByNameAsync(string name)
        {
            return await _db.Users.Where(u => u.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == email.ToLower().Trim());

        }
        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Trim() == phoneNumber.Trim());
        }

        public async Task<User> VerifyUser(LoginDTO user)
        {
            if(await EmailExistsAsync(user.Email)){
                return await _db.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
            }
            return null;
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }
    }
}
