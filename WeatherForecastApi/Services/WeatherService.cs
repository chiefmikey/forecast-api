using WeatherForecastApi.Models;
using Newtonsoft.Json;

namespace WeatherForecastApi.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiEndpoint;

        public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiEndpoint = configuration["WeatherApiEndpoint"];

            if (_apiEndpoint == null)
            {
                throw new InvalidOperationException("Missing configuration for 'WeatherApiEndpoint'");
            }
        }

        public virtual async Task<Weather> FetchWeatherData(double latitude, double longitude)
        {
            var requestUrl = CreateRequestUrl(latitude, longitude);
            var response = await SendHttpRequest(requestUrl);
            var weatherData = await DeserializeWeatherData(response);

            return weatherData;
        }

        private Uri CreateRequestUrl(double latitude, double longitude)
        {
            var builder = new UriBuilder(_apiEndpoint!)
            {
                Query = $"latitude={latitude}&longitude={longitude}&daily=temperature_2m_max,temperature_2m_min&temperature_unit=fahrenheit&wind_speed_unit=mph&precipitation_unit=inch"
            };

            return builder.Uri;
        }

        private async Task<HttpResponseMessage> SendHttpRequest(Uri requestUrl)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(requestUrl).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("An error occurred while making the HTTP request.", ex);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"The HTTP request returned a non-success status code: {response.StatusCode}");
            }

            return response;
        }

        private async Task<Weather> DeserializeWeatherData(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Weather? weatherData;
            try
            {
                weatherData = JsonConvert.DeserializeObject<Weather>(content);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("An error occurred while deserializing the weather data.", ex);
            }

            if (weatherData == null)
            {
                throw new InvalidOperationException("Unable to deserialize the response content to Weather data.");
            }

            return weatherData;
        }
    }
}
