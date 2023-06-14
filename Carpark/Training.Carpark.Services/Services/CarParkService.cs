using Microsoft.Extensions.Options;
using Training.Carpark.Repositories;
using Training.Carpark.Repositories.Models;
using Training.Carpark.Services.Models;

namespace Training.Carpark.Services.Services
{
    public class CarParkService : ICarparkService
    {
        public CarParkService(ICarparkRepository carparkRepository, IOptions<PricingSettings> configuration)
        {
            CarparkRepository = carparkRepository;
            Pricing = configuration.Value;
        }

        public ICarparkRepository CarparkRepository { get; set; }

        public PricingSettings Pricing { get; set; }

        public (ParkingSpaceServiceResponse, ParkingSpace) CreateParkingSpace(ParkingSpace parkingSpace)
        {
            if(String.IsNullOrEmpty(parkingSpace.Story) || parkingSpace.Number < 1)
            {
                return (ParkingSpaceServiceResponse.InvalidValue, null);
            }
            if (CarparkRepository.ParkingSpaceExists(parkingSpace.Number, parkingSpace.Story))
            {
                return (ParkingSpaceServiceResponse.AlreadyExists, null);
            }

            var repoParkingSpace = new RepoParkingSpace()
            {
                Number = parkingSpace.Number,
                Story = parkingSpace.Story,
                Id = Guid.NewGuid().ToString(),
            };

            var carparkRepoResponse = CarparkRepository.CreateParkingSpace(repoParkingSpace);

            return ((ParkingSpaceServiceResponse)carparkRepoResponse, repoParkingSpace.ToServiceParkingSpace());
        }

        public (ParkingSpaceServiceResponse, ParkingSpace) CheckinParkingSpace(string id)
        {
            if (CarparkRepository.ParkingSpaceExists(id))
            {
                if (CarparkRepository.IsOccupied(id))
                {
                    return (ParkingSpaceServiceResponse.AlreadyOccupied, null);
                }
                else
                {
                    var repoParkingSpace = new RepoParkingSpace()
                    {
                        Id = id,
                        Status = ParkingSpaceStatus.occupied.ToString(),
                        Timestamp = DateTime.UtcNow,
                    };

                    var carparkRepoResponse = CarparkRepository.CheckinParkingSpace(repoParkingSpace);

                    return ((ParkingSpaceServiceResponse)carparkRepoResponse, repoParkingSpace.ToServiceParkingSpace());
                }
            }

            return (ParkingSpaceServiceResponse.NoParkingSpaceFound, null);
        }

        public (ParkingSpaceServiceResponse, ParkingSpace) CheckoutParkingSpace(string id)
        {
            if (CarparkRepository.ParkingSpaceExists(id))
            {
                if (CarparkRepository.IsOccupied(id))
                {
                    var repoParkingSpace = CarparkRepository.CheckoutParkingSpace(id);
                    var serviceParkingSpace = repoParkingSpace.ToServiceParkingSpace();

                    serviceParkingSpace.DurationInHours = TimeOnly.FromTimeSpan(DateTime.UtcNow.Subtract(serviceParkingSpace.Timestamp));
                    serviceParkingSpace.Price = decimal.Add(serviceParkingSpace.Price, (decimal)(Pricing.Hourly * serviceParkingSpace.DurationInHours.Hour + Pricing.Fixed));

                    return (ParkingSpaceServiceResponse.Success, serviceParkingSpace);
                }
                else
                {
                    return (ParkingSpaceServiceResponse.AlreadyFree, null);
                }
            }
            return (ParkingSpaceServiceResponse.NoParkingSpaceFound, null);
        }


        public ParkingSpaceServiceResponse DeleteParkingSpace(string id)
        {
            if (CarparkRepository.ParkingSpaceExists(id))
            {
                var parkingSpaceRepoResponse = CarparkRepository.DeleteParkingSpace(id);
                return (ParkingSpaceServiceResponse)parkingSpaceRepoResponse;
            }

            return ParkingSpaceServiceResponse.NoParkingSpaceFound;

        }

        public (ParkingSpaceServiceResponse, ParkingSpace) GetParkingSpace(string id)
        {
            if (CarparkRepository.ParkingSpaceExists(id))
            {
                var (carparkRepoResponse, repoParkingSpace) = CarparkRepository.GetParkingSpace(id);

                return ((ParkingSpaceServiceResponse)carparkRepoResponse, repoParkingSpace.ToServiceParkingSpace());
            }

            return (ParkingSpaceServiceResponse.NoParkingSpaceFound, null);
        }

        public (ParkingSpaceServiceResponse, IEnumerable<ParkingSpace>) GetAllParkingSpaces()
        {
            var (parkingSpacesRepoResponse, repoParkingSpaces) = CarparkRepository.GetAllParkingSpaces();

            return ((ParkingSpaceServiceResponse)parkingSpacesRepoResponse, repoParkingSpaces.Select(x => x.ToServiceParkingSpace()));
        }
    }
}
