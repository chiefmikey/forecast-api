using Microsoft.EntityFrameworkCore;
using WeatherForecastApi.Models;
using Newtonsoft.Json;

namespace WeatherForecastApi.Data
{
    public class ForecastDbContext : DbContext
    {
        public ForecastDbContext(DbContextOptions<ForecastDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Forecast> Forecasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Forecast>()
                .Property(f => f.Daily)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Daily>(v));
        }
    }
}
