using TaskManager.Core.Enums;

namespace TaskManager.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy =>
                    policy.RequireClaim("UserType", UserType.Admin.ToString(), UserType.Admin.ToString()));

                options.AddPolicy("RequireUser", policy =>
                    policy.RequireClaim("UserType", UserType.User.ToString()));
            });

            return services;
        }
    }
}
