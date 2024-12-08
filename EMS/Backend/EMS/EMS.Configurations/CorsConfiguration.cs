public static class CorsConfiguration
{
    public static void AddCorsServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp", policy =>
            {
                policy.WithOrigins("http://localhost:4200") // Replace with your frontend URL
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}
