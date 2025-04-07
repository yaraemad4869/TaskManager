using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TaskManager.Core.Enums;
namespace TaskManager.Core.Models
{
    public class AppUser : IdentityUser
    {
        public UserType? userType { get; set; } = UserType.User;
    }
}
