using MongoDB.Driver;
using Training.Carpark.Api.Configuration;
using Training.Carpark.Repositories;
using Training.Carpark.Repositories.InMemory;
using Training.Carpark.Repositories.MongoDb;
using Training.Carpark.Services.Models;
using Training.Carpark.Services.Services;


var builder = WebApplication.CreateBuilder(args);
var mongoDbSettings = new MongoDBSettings();
var configDir = Environment.GetEnvironmentVariable("CONFIG_DIR");

builder.Configuration.AddJsonFile(Environment.CurrentDirectory + configDir);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvcCore();

builder.Services.AddScoped<ICarparkService, CarParkService>();

builder.Services.Configure<PricingSettings>(
    builder.Configuration.GetSection("Pricing"));

builder.Configuration.GetSection("Persistence").Bind(mongoDbSettings);


if (mongoDbSettings.Type == "mongodb")
{
    builder.Services.AddSingleton<ICarparkRepository, MongoCarparkRepository>();
    builder.Services.AddSingleton<IMongoDatabase>(x => new MongoClient(mongoDbSettings.Connectionstring).GetDatabase(mongoDbSettings.Databasename));
}
else
{
    builder.Services.AddSingleton<ICarparkRepository, InMemoryCarparkRepository>();
}


var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.UseCors("_myAllowSpecificOrigins");

app.Run();
