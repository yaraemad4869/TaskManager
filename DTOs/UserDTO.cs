using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.DTOs
{
    public class UserDTO
    {
        [RegularExpression(@"^[A-Za-z\s'-]+$",
         ErrorMessage = "Name can only contain letters, spaces, hyphens, and apostrophes.")]
        public string FName { get; set; }
        [RegularExpression(@"^[A-Za-z\s'-]+$",
        ErrorMessage = "Name can only contain letters, spaces, hyphens, and apostrophes.")]
        public string LName { get; set; }
        public DateOnly BirthDate { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address. Please use a valid email format (e.g., user@example.com).")]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*[a-z_])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least uppercase letter, lowercase letter, one number, and underscore.")]
        public string Password { get; set; }
        [RegularExpression(
            @"^\+?(\d{1,4})?[-. ]?\(?\d{1,4}\)?[-. ]?\d{1,4}[-. ]?\d{1,9}$",
            ErrorMessage = "Invalid phone number format."
        )]
        public string PhoneNumber { get; set; }
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{3,24}$",
    ErrorMessage = "Username must start with a letter, be 4-25 characters long, and can only contain letters, numbers and underscores.")]
        public string UserName { get; set; }
        public Gender gender { get; set; }

    }
}
