using Newtonsoft.Json;

namespace WeatherForecastApi.Models
{
    public class Weather
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("generationtime_ms")]
        public double GenerationTimeMs { get; set; }

        [JsonProperty("utc_offset_seconds")]
        public int UtcOffsetSeconds { get; set; }

        [JsonProperty("timezone")]
        public string? Timezone { get; set; }

        [JsonProperty("timezone_abbreviation")]
        public string? TimezoneAbbreviation { get; set; }

        [JsonProperty("elevation")]
        public double Elevation { get; set; }

        [JsonProperty("daily_units")]
        public DailyUnits? DailyUnits { get; set; }

        [JsonProperty("daily")]
        public Daily? Daily { get; set; }
    }

    public class DailyUnits
    {
        [JsonProperty("time")]
        public string? Time { get; set; }

        [JsonProperty("temperature_2m_max")]
        public string? Temperature2mMax { get; set; }

        [JsonProperty("temperature_2m_min")]
        public string? Temperature2mMin { get; set; }
    }

    public class Daily
    {
        [JsonProperty("time")]
        public List<string>? Time { get; set; }

        [JsonProperty("temperature_2m_max")]
        public List<double>? TemperatureMax { get; set; }

        [JsonProperty("temperature_2m_min")]
        public List<double>? TemperatureMin { get; set; }
    }
}
