
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
//builder.Services.AddDatabaseConnections(builder.Configuration);


builder.Services.AddSwaggerServices();
builder.Services.AddCorsServices();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())  
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
