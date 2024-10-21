#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y telnet

WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Archse.Worker/Archse.Worker.csproj", "Archse.Worker/"]
COPY ["Archse.Application/Archse.Application.csproj", "Archse.Application/"]
COPY ["Archse.Events/Archse.Events.csproj", "Archse.Events/"]
COPY ["Archse.Models/Archse.Models.csproj", "Archse.Models/"]
COPY ["Archse.Publisher/Archse.Publisher.csproj", "Archse.Publisher/"]
COPY ["Archse.Service/Archse.Service.csproj", "Archse.Service/"]
COPY ["Archse.Repository/Archse.Repository.csproj", "Archse.Repository/"]
COPY ["Archse.Cache/Archse.Cache.csproj", "Archse.Cache/"]
COPY ["Archse.Extensions/Archse.Extensions.csproj", "Archse.Extensions/"]
COPY ["Archse.Consumer/Archse.Consumer.csproj", "Archse.Consumer/"]
COPY ["Archse.Mapper/Archse.Mapper.csproj", "Archse.Mapper/"]
RUN dotnet restore "Archse.Worker/Archse.Worker.csproj"
COPY . .
WORKDIR "/src/Archse.Worker"
RUN dotnet build "Archse.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Archse.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Archse.Worker.dll"]