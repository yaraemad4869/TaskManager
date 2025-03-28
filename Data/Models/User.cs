using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Enums;

namespace TaskManager.Data.Models
{
    public class User
    {
        //public User() { 
        //    Items = new List<Item>();
        //    Categories = new List<Category>();
        //    Colors = new List<Color>();
        //}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [RegularExpression(@"^[A-Za-z\s'-]+$",
        ErrorMessage = "Name can only contain letters, spaces, hyphens, and apostrophes.")]
        public string FName { get; set; }
        [RegularExpression(@"^[A-Za-z\s'-]+$",
        ErrorMessage = "Name can only contain letters, spaces, hyphens, and apostrophes.")]
        public string LName { get; set; }
        public string Name => FName + ' ' + LName;
        public DateOnly BirthDate { get; set; }
        public int Age => (DateOnly.FromDateTime(DateTime.Now).DayNumber - BirthDate.DayNumber)/365;
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address. Please use a valid email format (e.g., user@example.com).")]
        public string Email { get; set; }
        [RegularExpression(@"^.{8,}$",
    ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }
        public UserType? userType { get; set; }=UserType.User;
        [RegularExpression(
            @"^\+?(\d{1,4})?[-. ]?\(?\d{1,4}\)?[-. ]?\d{1,4}[-. ]?\d{1,9}$",
            ErrorMessage = "Invalid phone number format."
        )]
        public string PhoneNumber { get; set; }
        public Gender gender { get; set; }
        public ICollection<Item>? Items { get; set; } = new List<Item>();
        public ICollection<Category>? Categories { get; set; } = new List<Category>();
        public ICollection<Color>? Colors { get; set; } = new List<Color>();
    }
}
