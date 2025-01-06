namespace EMS.Configurations
{
    public static class CorsConfiguration
    {
        public static void AddCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") 
                        .AllowAnyHeader()                       // Allow any headers
                        .AllowAnyMethod()                       // Allow any HTTP methods (GET, POST, PUT, DELETE, etc.)
                        .AllowCredentials();                    // Allow credentials (cookies, authorization headers, etc.)
                });
            });
        }
    }
}
