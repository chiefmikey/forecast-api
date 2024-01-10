using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using FluentAssertions;
using System.Threading;
using WeatherForecastApi.Services;
using WeatherForecastApi.Models;
using Moq;
using Moq.Protected;
using Microsoft.Extensions.Configuration;
using System;

namespace WeatherForecastApi.Tests
{
    public class WeatherServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private WeatherService _weatherService;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public WeatherServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _handlerMock = new Mock<HttpMessageHandler>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(_ => _["WeatherApiEndpoint"]).Returns("http://api.weather.com");

            var httpClient = new HttpClient(_handlerMock.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _weatherService = new WeatherService(_httpClientFactoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task FetchWeatherData_WhenCalled_ReturnsExpectedWeatherData()
        {
            // Arrange
            SetupSuccessfulResponse();
            var expectedWeatherData = new Weather
            {
                Latitude = 52.5200,
                Longitude = 13.4050,
                GenerationTimeMs = 1000.0,
                UtcOffsetSeconds = 3600,
                Timezone = "CET",
                TimezoneAbbreviation = "Central European Time",
                Elevation = 34.0,
                DailyUnits = new DailyUnits
                {
                    Time = "2022-01-01T00:00:00Z",
                    Temperature2mMax = "20.5",
                    Temperature2mMin = "10.5"
                },
                Daily = new Daily
                {
                    Time = new List<string> { "2022-01-01T00:00:00Z" },
                    TemperatureMax = new List<double> { 20.5 },
                    TemperatureMin = new List<double> { 10.5 }
                }
            };

            // Act
            var weatherData = await _weatherService.FetchWeatherData(52.5200, 13.4050);

            // Assert
            weatherData.Should().BeEquivalentTo(expectedWeatherData);
        }

        private void SetupSuccessfulResponse()
        {
            // Set up the handler mock to return a successful response
            _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"latitude\":52.5200,\"longitude\":13.4050,\"generationtime_ms\":1000.0,\"utc_offset_seconds\":3600,\"timezone\":\"CET\",\"timezone_abbreviation\":\"Central European Time\",\"elevation\":34.0,\"daily_units\":{\"time\":\"2022-01-01T00:00:00Z\",\"temperature_2m_max\":\"20.5\",\"temperature_2m_min\":\"10.5\"},\"daily\":{\"time\":[\"2022-01-01T00:00:00Z\"],\"temperature_2m_max\":[20.5],\"temperature_2m_min\":[10.5]}}"),
            });
        }

        [Fact]
        public async Task FetchWeatherData_WhenResponseIsUnsuccessful_ThrowsInvalidOperationException()
        {
            // Arrange
            SetupUnsuccessfulResponse();

            // Act
            Func<Task> act = async () => await _weatherService.FetchWeatherData(52.5200, 13.4050);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        private void SetupUnsuccessfulResponse()
        {
            // Set up the handler mock to return an unsuccessful response
            _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
            });
        }
    }
}
