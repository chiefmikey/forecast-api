using WeatherForecastApi.Middleware;
using WeatherForecastApi.Data;
using WeatherForecastApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Add this line
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>(); // Add this line
builder.Services.AddRazorPages().AddSessionStateTempDataProvider(); // Add this line
builder.Services.AddSession(); // Add this line

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ForecastDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Weather Forecast API", Version = "v1" });
});

// Add health checks
builder.Services.AddHealthChecks();

builder.Services.AddHttpClient();
builder.Services.AddScoped<WeatherService>();

var app = builder.Build();

// Use session middleware
app.UseSession(); // Add this line

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather Forecast API v1");
    });
}

// Use custom error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

// Configure CORS policy
app.UseCors(policy =>
    policy.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

// Map health checks
app.MapHealthChecks("/health");

app.Run();
