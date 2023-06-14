using Training.Carpark.Repositories.Models;

namespace Training.Carpark.Repositories.InMemory.Models
{
    public static class ParkingSpaceExtensions
    {
        public static RepoParkingSpace ToRepoCarSpace(this ParkingSpacePresistence parkingSpacePresistence)
        {
            var parkingSpace = new RepoParkingSpace()
            {
                Id = parkingSpacePresistence.Id,
                Number = parkingSpacePresistence.Number,
                Story = parkingSpacePresistence.Story,
                Status = parkingSpacePresistence.Status,
                Timestamp = parkingSpacePresistence.Timestamp,
            };

            return parkingSpace;
        }
        public static ParkingSpacePresistence ToInMemoryCarSpace(this RepoParkingSpace repoParkingSpace)
        {
            var parkingSpacePersistence = new ParkingSpacePresistence()
            {
                Id = repoParkingSpace.Id,
                Number = repoParkingSpace.Number,
                Story = repoParkingSpace.Story,
                Status = repoParkingSpace.Status,
                Timestamp = repoParkingSpace.Timestamp,
            };

            return parkingSpacePersistence;
        }
    }
}
