using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using TaskManager.Core.Enums;

public class AuthorizeUserTypeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly UserType[] _allowedUserTypes;

    public AuthorizeUserTypeAttribute(params UserType[] allowedUserTypes)
    {
        _allowedUserTypes = allowedUserTypes;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            // User is not authenticated
            context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            return;
        }

        var userTypeClaim = user.Claims.FirstOrDefault(c => c.Type == "UserType");
        if (userTypeClaim == null)
        {
            // UserType claim not found
            context.Result = new JsonResult(new { message = "Forbidden - UserType claim missing" })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        if (!_allowedUserTypes.Any(ut => ut.ToString() == userTypeClaim.Value))
        {
            // User doesn't have the required UserType
            context.Result = new JsonResult(new { message = "Forbidden - Insufficient privileges" })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}