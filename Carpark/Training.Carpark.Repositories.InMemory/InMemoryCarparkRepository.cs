using Training.Carpark.Repositories.InMemory.Models;
using Training.Carpark.Repositories.Models;

namespace Training.Carpark.Repositories.InMemory
{
    public class InMemoryCarparkRepository : ICarparkRepository
    {
        public InMemoryCarparkRepository()
        {
            carParkInMemoryPersistence = new CarParkInMemoryPersistence();
        }

        public CarParkInMemoryPersistence carParkInMemoryPersistence { get; set; }

        public ParkingSpaceRepoResponse CreateParkingSpace(RepoParkingSpace repoParkingSpace)
        {
            var parkingSpacePresistence = repoParkingSpace.ToInMemoryCarSpace();

            carParkInMemoryPersistence.ParkingSpaces.Add(parkingSpacePresistence.Id, parkingSpacePresistence);

            return ParkingSpaceRepoResponse.Success;
        }

        public ParkingSpaceRepoResponse DeleteParkingSpace(string id)
        {
            carParkInMemoryPersistence.ParkingSpaces.Remove(id);

            return ParkingSpaceRepoResponse.Success;
        }

        public (ParkingSpaceRepoResponse, IEnumerable<RepoParkingSpace>) GetAllParkingSpaces()
        {
            var parkingSpaces = carParkInMemoryPersistence.ParkingSpaces.Select(x => x.Value.ToRepoCarSpace()).ToArray();

            return (ParkingSpaceRepoResponse.Success, parkingSpaces);
        }

        public (ParkingSpaceRepoResponse, RepoParkingSpace) GetParkingSpace(string id)
        {
            var parkingSpace = carParkInMemoryPersistence.ParkingSpaces[id];

            return (ParkingSpaceRepoResponse.Success, parkingSpace.ToRepoCarSpace());
        }

        public ParkingSpaceRepoResponse CheckinParkingSpace(RepoParkingSpace repoParkingSpace)
        {
            var parkingSpacePresistence = repoParkingSpace.ToInMemoryCarSpace();

            carParkInMemoryPersistence.ParkingSpaces[parkingSpacePresistence.Id].Status = parkingSpacePresistence.Status;
            carParkInMemoryPersistence.ParkingSpaces[parkingSpacePresistence.Id].Timestamp = parkingSpacePresistence.Timestamp;

            return ParkingSpaceRepoResponse.Success;
        }
        public RepoParkingSpace CheckoutParkingSpace(string id)
        {
            carParkInMemoryPersistence.ParkingSpaces[id].Status = "free";
            return carParkInMemoryPersistence.ParkingSpaces[id].ToRepoCarSpace();
        }

        public bool ParkingSpaceExists(string id)
        {
            ParkingSpacePresistence? parkingSpace;

            carParkInMemoryPersistence.ParkingSpaces.TryGetValue(id, out parkingSpace);

            return parkingSpace != null;
        }

        public bool ParkingSpaceExists(int number, string story)
        {
            var parkingSpace = carParkInMemoryPersistence.ParkingSpaces.FirstOrDefault(x => x.Value.Number == number && x.Value.Story == story);

            return parkingSpace.Value != null;
        }

        public bool IsOccupied(string id)
        {
            return carParkInMemoryPersistence.ParkingSpaces[id].Status == "occupied";
        }
    }
}
