using Microsoft.AspNetCore.Mvc;
using WeatherForecastApi.Data;
using WeatherForecastApi.Models;
using WeatherForecastApi.Services;
using Microsoft.EntityFrameworkCore;

namespace WeatherForecastApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForecastController : Controller
    {
        private readonly ForecastDbContext _context;
        private readonly WeatherService _weatherService;

        public ForecastController(ForecastDbContext context, WeatherService weatherService, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Forecast>>> GetAll()
        {
            return await _context.Forecasts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Forecast>> GetById(int id)
        {
            var forecast = await _context.Forecasts.FindAsync(id);
            if (forecast == null)
            {
                return NotFound();
            }
            return forecast;
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateWeather(int id)
        {
            var forecast = await _context.Forecasts.FindAsync(id);

            if (forecast == null)
            {
                return NotFound();
            }

            var weatherData = await _weatherService.FetchWeatherData(forecast.Latitude, forecast.Longitude);

            forecast.Daily = weatherData.Daily;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/refresh")]
        public async Task<ActionResult<Forecast>> RefreshWeather(int id)
        {
            var forecast = await _context.Forecasts.FindAsync(id);

            if (forecast == null)
            {
                return NotFound();
            }

            var weatherData = await _weatherService.FetchWeatherData(forecast.Latitude, forecast.Longitude);

            forecast.Daily = weatherData.Daily;

            await _context.SaveChangesAsync();

            return forecast;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHtml()
        {
            var forecasts = await _context.Forecasts.ToListAsync();
            return View("History", forecasts);
        }

        [HttpPost]
        public async Task<ActionResult<Forecast>> Create(Forecast forecast)
        {
            var weatherData = await _weatherService.FetchWeatherData(forecast.Latitude, forecast.Longitude);

            forecast.Daily = weatherData.Daily;
            Console.WriteLine(weatherData);

            _context.Forecasts.Add(forecast);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = forecast.Id }, forecast);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var forecast = await _context.Forecasts.FindAsync(id);
            if (forecast == null)
            {
                return NotFound();
            }

            _context.Forecasts.Remove(forecast);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
