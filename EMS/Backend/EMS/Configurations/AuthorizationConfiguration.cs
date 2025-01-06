using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Configurations
{
    public static class AuthorizationConfiguration
    {
        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserPolicy", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("User") || context.User.IsInRole("0") ||

                        context.User.IsInRole("Admin") || context.User.IsInRole("1") ||

                        context.User.IsInRole("SuperAdmin") || context.User.IsInRole("2"));
                });

                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Admin") || context.User.IsInRole("1") ||

                        context.User.IsInRole("SuperAdmin") || context.User.IsInRole("2"));
                });

                options.AddPolicy("SuperAdminPolicy", policy =>
                {
                    policy.RequireAssertion(context =>

                        context.User.IsInRole("SuperAdmin") || context.User.IsInRole("2"));
                });
            });
        }
    }
}
