using TaskManager.Core.Enums;

namespace TaskManager.Core.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserType UserType { get; set; }
    }
}
