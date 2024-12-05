using EMS.Data.Contexts;
using EMS.Repositories.Interfaces;
using EMS.Repositories.Implementations;
using EMS.Services.Interfaces;
using EMS.Services.Implementations;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register repositories and services
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<IDesignationService, DesignationService>();

builder.Services.AddScoped<IOperationLogRepository, OperationLogRepository>();
builder.Services.AddScoped<IOperationLogService, OperationLogService>();

// Register SqlConnection with the connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
builder.Services.AddScoped<SqlConnection>(_ => new SqlConnection(connectionString));

// Register MongoClient as Singleton
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDbConnection")));

// Add controllers
builder.Services.AddControllers();

// Register Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "EMS API",
        Version = "v1",
        Description = "An API for managing employees, departments, and designations."
    });
});

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Replace with your frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())  // Enable Swagger in Production if needed
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS in the middleware pipeline
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
