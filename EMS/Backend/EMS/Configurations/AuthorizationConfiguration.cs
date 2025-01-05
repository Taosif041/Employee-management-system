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
                options.AddPolicy("UserPolicy", policy => policy.RequireRole("0"));
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("1"));
                options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("2"));
            });
        }
    }

}
