using MongoDB.Bson.Serialization.Attributes;

namespace Training.Carpark.Repositories.InMemory.Models
{
    public class CarParkMongoPersistenceModel
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("number")]
        public int Number { get; set; }

        [BsonElement("story")]
        public string Story { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
