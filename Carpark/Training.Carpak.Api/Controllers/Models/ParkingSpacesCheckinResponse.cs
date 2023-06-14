using Newtonsoft.Json;

namespace Training.Carpark.Api.Controllers.Models
{
    public class ParkingSpacesCheckinResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp { get; set; } = string.Empty;
    }
}
