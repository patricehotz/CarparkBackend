using MongoDB.Driver;
using Training.Carpark.Repositories.InMemory.Models;
using Training.Carpark.Repositories.Models;

namespace Training.Carpark.Repositories.MongoDb
{
    public class MongoCarparkRepository : ICarparkRepository
    {
        private readonly IMongoCollection<CarParkMongoPersistenceModel> _parkingSpaces;

        public MongoCarparkRepository(IMongoDatabase mongoDatabase)
        {
            _parkingSpaces = mongoDatabase.GetCollection<CarParkMongoPersistenceModel>("parkingspaces");
        }

        public ParkingSpaceRepoResponse CheckinParkingSpace(RepoParkingSpace repoParkingSpace)
        {
            var parkingSpace = repoParkingSpace.ToMongoPersistenceModel();

            var response = _parkingSpaces.UpdateOne(ParkingSpaceById(parkingSpace.Id), UpdateParkingSpaceStatusAndTimeStamp(parkingSpace.Status, parkingSpace.Timestamp));

            return response.ModifiedCount == 1 ?  ParkingSpaceRepoResponse.Success : ParkingSpaceRepoResponse.Unsuccessful;
        }

        public RepoParkingSpace CheckoutParkingSpace(string id)
        {
            _parkingSpaces.UpdateOne(ParkingSpaceById(id), UpdateParkingSpaceStatus("free"));

            var parkingSpace = _parkingSpaces.Find(ParkingSpaceById(id)).First();

            return parkingSpace.ToRepoCarSpace();
        }

        public ParkingSpaceRepoResponse CreateParkingSpace(RepoParkingSpace repoParkingSpace)
        {
            var parkingSpaceQuantity = _parkingSpaces.CountDocuments(ParkingSpacesAll());

            _parkingSpaces.InsertOne(repoParkingSpace.ToMongoPersistenceModel());

            if (_parkingSpaces.CountDocuments(ParkingSpacesAll()) == parkingSpaceQuantity + 1)
            {
                return ParkingSpaceRepoResponse.Success;
            }
            return ParkingSpaceRepoResponse.Unsuccessful;
        }

        public ParkingSpaceRepoResponse DeleteParkingSpace(string id)
        {
            var result = _parkingSpaces.DeleteOne(ParkingSpaceById(id));

            if (result.DeletedCount == 1)
            {
                return ParkingSpaceRepoResponse.Success;
            }
            return ParkingSpaceRepoResponse.Unsuccessful;
        }

        public (ParkingSpaceRepoResponse, IEnumerable<RepoParkingSpace>) GetAllParkingSpaces()
        {
            var parkingSpaces = _parkingSpaces.Find(ParkingSpacesAll()).ToList();

            return (ParkingSpaceRepoResponse.Success, parkingSpaces.Select(x => x.ToRepoCarSpace()).ToArray());
        }

        public (ParkingSpaceRepoResponse, RepoParkingSpace) GetParkingSpace(string id)
        {
            var parkingSpace = _parkingSpaces.Find(ParkingSpaceById(id)).First();

            return (ParkingSpaceRepoResponse.Success, parkingSpace.ToRepoCarSpace());
        }

        public bool IsOccupied(string id) => _parkingSpaces.Find(ParkingSpaceById(id)).First().Status == "occupied";

        public bool ParkingSpaceExists(string id) => _parkingSpaces.Find(ParkingSpaceById(id)).FirstOrDefault() != null;

        public bool ParkingSpaceExists(int number, string story) => _parkingSpaces.Find(ParkingSpaceByNumberAndStory(number, story)).FirstOrDefault() != null;

        private static FilterDefinition<CarParkMongoPersistenceModel> ParkingSpacesAll() => Builders<CarParkMongoPersistenceModel>.Filter.Empty;

        private static FilterDefinition<CarParkMongoPersistenceModel> ParkingSpaceById(string id) => Builders<CarParkMongoPersistenceModel>.Filter.Eq(ps => ps.Id, id);

        private static FilterDefinition<CarParkMongoPersistenceModel> ParkingSpaceByNumber(int number) => Builders<CarParkMongoPersistenceModel>.Filter.Eq(ps => ps.Number, number);

        private static FilterDefinition<CarParkMongoPersistenceModel> ParkingSpaceByStory(string story) => Builders<CarParkMongoPersistenceModel>.Filter.Eq(ps => ps.Story, story);

        private static FilterDefinition<CarParkMongoPersistenceModel> ParkingSpaceByNumberAndStory(int number, string story) => Builders<CarParkMongoPersistenceModel>.Filter.And(ParkingSpaceByNumber(number), ParkingSpaceByStory(story));

        private static UpdateDefinition<CarParkMongoPersistenceModel> UpdateParkingSpaceStatus(string status) => Builders<CarParkMongoPersistenceModel>.Update.Set(ps => ps.Status, status);

        private static UpdateDefinition<CarParkMongoPersistenceModel> UpdateParkingSpaceTimeStamp(DateTime timeStamp) => Builders<CarParkMongoPersistenceModel>.Update.Set(ps => ps.Timestamp, timeStamp);

        private static UpdateDefinition<CarParkMongoPersistenceModel> UpdateParkingSpaceStatusAndTimeStamp(string status, DateTime timeStamp) => Builders<CarParkMongoPersistenceModel>.Update.Combine(UpdateParkingSpaceStatus(status), UpdateParkingSpaceTimeStamp(timeStamp));
    }
}
