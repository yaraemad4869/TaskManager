using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using TaskManager.Core.Models;
using TaskManager.DTOs;

namespace TaskManager.Core.Interfaces
{
    public interface IUserRepo
    {
        Task<List<User>> GetUserByNameAsync(string name);
        Task<User> VerifyUser(LoginDTO user);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<bool> EmailExistsAsync(string email);

    }
}
