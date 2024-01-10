using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastApi.Data;
using WeatherForecastApi.Models;
using WeatherForecastApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System;
using WeatherForecastApi.Controllers;
using Microsoft.Extensions.Configuration;
using Moq.Protected;
using System.Net;

namespace WeatherForecastApi.Tests
{
    public class ForecastControllerTests : IDisposable
    {
        private ForecastController _forecastController;
        private ForecastDbContext _context;
        private Mock<WeatherService> _weatherServiceMock;
        private Mock<IWebHostEnvironment> _hostingEnvironmentMock;

        public ForecastControllerTests()
        {
            var options = new DbContextOptionsBuilder<ForecastDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ForecastDbContext(options);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(_ => _["WeatherApiEndpoint"]).Returns("http://api.weather.com");

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'key1':'value1'}]"),
                });

            var httpClientWithHandler = new HttpClient(handlerMock.Object);
            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClientWithHandler);

            _weatherServiceMock = new Mock<WeatherService>(httpClientFactoryMock.Object, configurationMock.Object);
            _hostingEnvironmentMock = new Mock<IWebHostEnvironment>();

            _forecastController = new ForecastController(_context, _weatherServiceMock.Object, _hostingEnvironmentMock.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetAll_ReturnsAllForecasts()
        {
            // Arrange
            var forecasts = new List<Forecast>
            {
                new Forecast { Id = 1, Latitude = 52.5200, Longitude = 13.4050 },
                new Forecast { Id = 2, Latitude = 40.7128, Longitude = 74.0060 }
            };

            _context.Forecasts.AddRange(forecasts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _forecastController.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<Forecast>>>(result);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(2, actionResult.Value?.Count);
        }

        [Fact]
        public async Task GetById_ReturnsForecast_WhenForecastExists()
        {
            // Arrange
            var forecast = new Forecast { Id = 1, Latitude = 52.5200, Longitude = 13.4050 };
            _context.Forecasts.Add(forecast);
            await _context.SaveChangesAsync();

            // Act
            var result = await _forecastController.GetById(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Forecast>>(result);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(1, actionResult.Value?.Id);
        }

        [Fact]
        public async Task UpdateWeather_ReturnsNoContent_WhenForecastExists()
        {
            // Arrange
            var forecast = new Forecast { Id = 1, Latitude = 52.5200, Longitude = 13.4050 };
            _context.Forecasts.Add(forecast);
            await _context.SaveChangesAsync();

            var weatherData = new Weather { /* Initialize weather data here */ };

            _weatherServiceMock
                .Setup(service => service.FetchWeatherData(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _forecastController.UpdateWeather(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RefreshWeather_ReturnsForecast_WhenForecastExists()
        {
            // Arrange
            var forecast = new Forecast { Id = 1, Latitude = 52.5200, Longitude = 13.4050 };
            _context.Forecasts.Add(forecast);
            await _context.SaveChangesAsync();

            var weatherData = new Weather
            {
                Daily = new Daily
                {
                    Time = new List<string> { "10:00", "14:00", "18:00" },
                    TemperatureMax = new List<double> { 20.0, 22.0, 18.0 },
                    TemperatureMin = new List<double> { 10.0, 12.0, 8.0 }
                }
            };

            _weatherServiceMock
                .Setup(service => service.FetchWeatherData(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(weatherData);

            // Act
            var result = await _forecastController.RefreshWeather(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Forecast>>(result);
            var returnedForecast = Assert.IsType<Forecast>(actionResult.Value);
            Assert.Equal(forecast.Daily, returnedForecast.Daily);
        }
    }
}
