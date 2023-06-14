using Newtonsoft.Json;

namespace Training.Carpark.Api.Controllers.Models
{
    public class ParkingSpacesCheckoutResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; } = string.Empty;
    }
}
