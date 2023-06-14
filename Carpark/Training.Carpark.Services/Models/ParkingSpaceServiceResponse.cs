namespace Training.Carpark.Services.Models
{
    public enum ParkingSpaceServiceResponse
    {
        NoParkingSpaceFound,
        Success,
        Unsuccessful,
        AlreadyExists,
        AlreadyOccupied,
        AlreadyFree,
        InvalidValue
    }
}
