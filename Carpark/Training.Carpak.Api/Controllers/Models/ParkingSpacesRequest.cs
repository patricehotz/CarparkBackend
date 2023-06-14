using Newtonsoft.Json;

namespace Training.Carpark.Api.Controllers.Models
{
    public class ParkingSpacesRequest
    {
        [JsonProperty(PropertyName = "story")]
        public string Story { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }
    }
}
