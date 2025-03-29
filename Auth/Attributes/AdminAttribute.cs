using TaskManager.Core.Enums;

namespace TaskManager.Auth.Attributes
{
    public class AdminAttribute : AuthorizeUserTypeAttribute
    {
        public AdminAttribute() : base(UserType.Admin)
        {
        }
    }
}
