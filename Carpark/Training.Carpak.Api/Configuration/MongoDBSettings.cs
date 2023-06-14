using Newtonsoft.Json;

namespace Training.Carpark.Api.Configuration
{
    public class MongoDBSettings
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "connectionstring")]
        public string Connectionstring { get; set; }

        [JsonProperty(PropertyName = "databasename")]
        public string Databasename { get; set; }
    }
}
