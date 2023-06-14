#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Training.Carpak.Api/Training.Carpark.Api.csproj", "Training.Carpak.Api/"]
COPY ["Training.Carpark.Repositories.InMemory/Training.Carpark.Repositories.InMemory.csproj", "Training.Carpark.Repositories.InMemory/"]
COPY ["Training.Carpark.Repositories/Training.Carpark.Repositories.csproj", "Training.Carpark.Repositories/"]
COPY ["Training.Carpark.Services/Training.Carpark.Services.csproj", "Training.Carpark.Services/"]
COPY ["Training.Carpark.Repositories.MongoDb/Training.Carpark.Repositories.MongoDb.csproj", "Training.Carpark.Repositories.MongoDb/"]
RUN dotnet restore "Training.Carpak.Api/Training.Carpark.Api.csproj"
COPY . .
WORKDIR "/src/Training.Carpak.Api"
RUN dotnet build "Training.Carpark.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Training.Carpark.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Training.Carpark.Api.dll"]