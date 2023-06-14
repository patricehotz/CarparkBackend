using Training.Carpark.Repositories.Models;

namespace Training.Carpark.Repositories
{
    public interface ICarparkRepository
    {
        public ParkingSpaceRepoResponse CreateParkingSpace(RepoParkingSpace repoParkingSpace);
        public ParkingSpaceRepoResponse CheckinParkingSpace(RepoParkingSpace repoParkingSpace);
        public RepoParkingSpace CheckoutParkingSpace(string id);
        public ParkingSpaceRepoResponse DeleteParkingSpace(string id);
        public (ParkingSpaceRepoResponse, RepoParkingSpace) GetParkingSpace(string id);
        public (ParkingSpaceRepoResponse, IEnumerable<RepoParkingSpace>) GetAllParkingSpaces();
        public bool ParkingSpaceExists(string id);
        public bool ParkingSpaceExists(int number, string story);
        public bool IsOccupied(string id);
    }
}
