using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Training.Carpark.Api.Controllers.Models;
using Training.Carpark.Services.Models;
using Training.Carpark.Services.Services;

namespace Training.Carpark.Api.Controllers
{
    [Route("v1/spaces/")]
    public class ParkingSpacesController : Controller
    {
        public ParkingSpacesController(ICarparkService carparkService)
        {
            CarparkService = carparkService;
        }

        public ICarparkService CarparkService { get; set; }

        [HttpPost]
        public IActionResult CreateParkingSpace([FromBody] ParkingSpacesRequest parkingSpaceRequest)
         {
            var serviceParkingSpace = new ParkingSpace() { Number = parkingSpaceRequest.Number, Story = parkingSpaceRequest.Story };
            var (parkingSpaceServiceResponse, parkingSpace) = CarparkService.CreateParkingSpace(serviceParkingSpace);

            switch (parkingSpaceServiceResponse)
            {
                case ParkingSpaceServiceResponse.Success:
                    {
                        var parkingSpaceResponse = new ParkingSpacesResponse()
                        {
                            Id = parkingSpace.Id,
                            Number = parkingSpace.Number,
                            Story = parkingSpace.Story,
                            Status = parkingSpace.Status.ToString(),
                        };

                        return Ok(parkingSpaceResponse);
                    }
                case ParkingSpaceServiceResponse.AlreadyExists:
                    {
                        return Conflict("A parking space with the supplied number and story already exists in the system.");
                    }
                case ParkingSpaceServiceResponse.InvalidValue:
                    {
                        return BadRequest("Not all properties in the request have a valid value.");
                    }
                default:
                    {
                        return StatusCode(500, "Something went wrong while creating parking space");
                    }
            }
        }

        [HttpPost("{id}/checkin")]
        public IActionResult CheckinParkingSpace([FromRoute] string id)
        {
            var (parkingSpaceServiceResponse, parkingSpace) = CarparkService.CheckinParkingSpace(id);

            switch (parkingSpaceServiceResponse)
            {
                case ParkingSpaceServiceResponse.Success:
                    {
                        var parkingSpaceCheckinResponse = new ParkingSpacesCheckinResponse()
                        {
                            Id = parkingSpace.Id,
                            Timestamp = parkingSpace.Timestamp.ToString("MM.dd.yyyy H:mm")
                        };

                        return Ok(parkingSpaceCheckinResponse);
                    }

                case ParkingSpaceServiceResponse.NoParkingSpaceFound:
                    {
                        return NotFound("No parking space was found for the ID.");
                    }
                case ParkingSpaceServiceResponse.AlreadyOccupied:
                    {
                        return BadRequest("The parking space is already occupied.");
                    }
                default:
                    {
                        return StatusCode(500, "Something went wrong while checking in parking space");
                    }
            }
        }

        [HttpPost("{id}/checkout")]
        public IActionResult CheckoutParkingSpace([FromRoute] string id)
        {
            var (parkingSpaceServiceResponse, parkingSpace) = CarparkService.CheckoutParkingSpace(id);

            switch (parkingSpaceServiceResponse)
            {
                case ParkingSpaceServiceResponse.Success:
                    {
                        var parkingSpaceCheckoutResponse = new ParkingSpacesCheckoutResponse()
                        {
                            Id = parkingSpace.Id,
                            Duration = parkingSpace.DurationInHours.ToString("HH:mm"),
                            Price = $"CHF {parkingSpace.Price:00.00}",
                        };

                        return Ok(parkingSpaceCheckoutResponse);
                    }

                case ParkingSpaceServiceResponse.NoParkingSpaceFound:
                    {
                        return NotFound("No parking space was found for the ID.");
                    }
                case ParkingSpaceServiceResponse.AlreadyFree:
                    {
                        return BadRequest("The parking space is not currently occupied.");
                    }
                default:
                    {
                        return StatusCode(500);
                    }
            }
        }

        [HttpGet]
        public IActionResult GetParkingSpaces()
        {
            var (parkingSpaceServiceResponse, parkingSpaces) = CarparkService.GetAllParkingSpaces();

            switch (parkingSpaceServiceResponse)
            {
                case ParkingSpaceServiceResponse.Success:
                    {
                        var parkingSpaceResponses = parkingSpaces.Select(
                                x => new ParkingSpacesResponse()
                                {
                                    Id = x.Id,
                                    Story = x.Story,
                                    Number = x.Number,
                                    Status = x.Status.ToString(),
                                }).ToArray();

                        return Ok(parkingSpaceResponses);
                    }
                default:
                    {
                        return StatusCode(500, "Something went wrong while getting parking spaces");
                    }
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetParkingSpace([FromRoute] string id)
        {
            var (parkingSpaceServiceResponse, parkingSpace) = CarparkService.GetParkingSpace(id);

            switch (parkingSpaceServiceResponse)
            {
                case ParkingSpaceServiceResponse.Success:
                    {
                        var parkingSpaceResponse = new ParkingSpacesResponse()
                        {
                            Id = parkingSpace.Id,
                            Story = parkingSpace.Story,
                            Number = parkingSpace.Number,
                            Status = parkingSpace.Status.ToString(),
                        };

                        return Ok(parkingSpaceResponse);
                    }
                case ParkingSpaceServiceResponse.NoParkingSpaceFound:
                    {
                        return NotFound("No parking space was found for the ID.");
                    }
                default:
                    {
                        return StatusCode(500, "Something went wrong while getting parking space");
                    }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteParkingSpace([FromRoute] string id)
        {
            var parkingSpaceServiceResponse = CarparkService.DeleteParkingSpace(id);

            switch (parkingSpaceServiceResponse)
            {
                case ParkingSpaceServiceResponse.Success:
                    {
                        return NoContent();
                    }
                case ParkingSpaceServiceResponse.NoParkingSpaceFound:
                    {
                        return NotFound("No parking space was found for the ID.");
                    }
                default:
                    {
                        return StatusCode(500, "Something went wrong while deleteing parking space");
                    }
            }
        }
    }
}
