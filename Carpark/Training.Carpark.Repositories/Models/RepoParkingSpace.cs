namespace Training.Carpark.Repositories.Models
{
    public class RepoParkingSpace
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string Story { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
