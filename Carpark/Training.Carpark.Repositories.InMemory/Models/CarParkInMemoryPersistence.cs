namespace Training.Carpark.Repositories.InMemory.Models
{
    public class CarParkInMemoryPersistence
    {
        public Dictionary<string, ParkingSpacePresistence> ParkingSpaces { get; set; } = new Dictionary<string, ParkingSpacePresistence>();
    }
}
