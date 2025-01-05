using EMS.Configurations;
using EMS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerServices(); // Register Swagger configuration
builder.Services.AddCorsServices();    // Register CORS configuration
builder.Services.AddControllers();

// Configure JwtSettings and JWT Authentication
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddJwtAuthentication(builder.Configuration); // Add JWT Authentication
builder.Services.AddAuthorizationPolicies(); // Add Authorization configuration


var app = builder.Build();

// Use Swagger and SwaggerUI
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
