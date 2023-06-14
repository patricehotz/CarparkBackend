namespace Training.Carpark.Services.Models
{
    public static class ParkinSpaceExtension
    {
        public static ParkingSpace ToServiceParkingSpace(this Repositories.Models.RepoParkingSpace repoParkingSpace)
        {
            if (repoParkingSpace != null)
            {
                var serviceParkingSpace = new ParkingSpace()
                {
                    Id = repoParkingSpace.Id,
                    Number = repoParkingSpace.Number,
                    Story = repoParkingSpace.Story,
                    Timestamp = repoParkingSpace.Timestamp,
                };

                if (repoParkingSpace.Status == "free")
                {
                    serviceParkingSpace.Status = ParkingSpaceStatus.free;
                }
                if (repoParkingSpace.Status == "occupied")
                {
                    serviceParkingSpace.Status = ParkingSpaceStatus.occupied;
                }

                return serviceParkingSpace;
            }
            else
            {
                return null;
            }
        }
    }
}
