using WeatherForecastApi.Middleware;
using WeatherForecastApi.Data;
using WeatherForecastApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
builder.Services.AddSession();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ForecastDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Weather Forecast API", Version = "v1" });
});

builder.Services.AddHealthChecks();

builder.Services.AddHttpClient();
builder.Services.AddScoped<WeatherService>();

var app = builder.Build();

app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather Forecast API v1");
    });
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(policy =>
    policy.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
