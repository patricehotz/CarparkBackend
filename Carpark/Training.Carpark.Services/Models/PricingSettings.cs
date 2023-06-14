using Newtonsoft.Json;

namespace Training.Carpark.Services.Models
{
    public class PricingSettings
    {
        [JsonProperty(PropertyName = "hourly")]
        public float Hourly { get; set; }

        [JsonProperty(PropertyName = "fixed")]
        public float Fixed { get; set; }
    }
}

