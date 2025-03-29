using Microsoft.AspNetCore.Authorization;
using System.Data;
using TaskManager.Core.Enums;

namespace TaskManager.Auth.Attributes
{
    public class AuthorizeUserTypeAttribute : AuthorizeAttribute
    {
        public AuthorizeUserTypeAttribute(params UserType[] allowedUserTypes)
        {
            var allowedTypesAsStrings = allowedUserTypes.Select(x => x.ToString());
            Roles = string.Join(",", allowedTypesAsStrings);
        }
    }
}
