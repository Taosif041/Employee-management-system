using Microsoft.OpenApi.Models;

public static class SwaggerConfiguration
{
    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "EMS API",
                Version = "v1",
                Description = "An API for managing employees, departments, and designations."
            });
        });
    }
}
