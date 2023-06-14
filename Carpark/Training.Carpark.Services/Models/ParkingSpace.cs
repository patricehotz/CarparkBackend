namespace Training.Carpark.Services.Models
{
    public class ParkingSpace
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string Story { get; set; } = string.Empty;
        public ParkingSpaceStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeOnly DurationInHours { get; set; }
        public Decimal Price { get; set; }

    }
}
