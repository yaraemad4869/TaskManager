using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DTOs
{
    public class LoginDTO
    {
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address. Please use a valid email format (e.g., user@example.com).")]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*[a-z_])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least uppercase letter, lowercase letter, one number, and underscore.")]

        public string Password { get; set; }
    }
}
