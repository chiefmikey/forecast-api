using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WeatherForecastApi.Models
{
    public class Forecast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [Required]
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [NotMapped]
        [JsonProperty("daily")]
        public Daily? Daily { get; set; }
    }
}
