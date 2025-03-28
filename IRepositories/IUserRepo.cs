using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using TaskManager.Data.DTOs;
using TaskManager.Data.Models;

namespace TaskManager.IRepositories
{
    public interface IUserRepo
    {
        Task<List<User>> GetUserByNameAsync(string name, params Func<IQueryable<User>?, IIncludableQueryable<User, object>>[]? includes);
        Task<User> VerifyUser(LoginDTO user, params Func<IQueryable<User>?, IIncludableQueryable<User, object>>[]? includes);
        Task<User> GetUserByEmailAsync(string email, params Func<IQueryable<User>?, IIncludableQueryable<User, object>>[]? includes);
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber, params Func<IQueryable<User>?, IIncludableQueryable<User, object>>[]? includes);

    }
}
