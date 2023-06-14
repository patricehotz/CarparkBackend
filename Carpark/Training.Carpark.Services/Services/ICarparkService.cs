using Training.Carpark.Services.Models;

namespace Training.Carpark.Services.Services
{
    public interface ICarparkService
    {
        public (ParkingSpaceServiceResponse, ParkingSpace) CreateParkingSpace(ParkingSpace parkingSpace);
        public (ParkingSpaceServiceResponse, ParkingSpace) CheckinParkingSpace(string id);
        public (ParkingSpaceServiceResponse, ParkingSpace) CheckoutParkingSpace(string id);
        public ParkingSpaceServiceResponse DeleteParkingSpace(string id);
        public (ParkingSpaceServiceResponse, ParkingSpace) GetParkingSpace(string id);
        public (ParkingSpaceServiceResponse, IEnumerable<ParkingSpace>) GetAllParkingSpaces();
    }
}
