using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data.DTOs
{
    public class LoginDTO
    {
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address. Please use a valid email format (e.g., user@example.com).")]
        public string Email { get; set; }
        [RegularExpression(@"^.{8,}$",
    ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }
    }
}
