using Newtonsoft.Json;

namespace Training.Carpark.Api.Controllers.Models
{
    public class ParkingSpacesResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "story")]
        public string Story { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; } = string.Empty;
    }
}
