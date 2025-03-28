using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Enums;

namespace TaskManager.Data.DTOs
{
    public class UserDTO
    {
        [RegularExpression(@"^[A-Za-z\s'-]+$",
         ErrorMessage = "Name can only contain letters, spaces, hyphens, and apostrophes.")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[A-Za-z\s'-]+$",
        ErrorMessage = "Name can only contain letters, spaces, hyphens, and apostrophes.")]
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address. Please use a valid email format (e.g., user@example.com).")]
        public string Email { get; set; }
        [RegularExpression(@"^.{8,}$",
    ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }
        [RegularExpression(
            @"^\+?(\d{1,4})?[-. ]?\(?\d{1,4}\)?[-. ]?\d{1,4}[-. ]?\d{1,9}$",
            ErrorMessage = "Invalid phone number format."
        )]
        public string PhoneNumber { get; set; }
        public Gender gender { get; set; }

    }
}
