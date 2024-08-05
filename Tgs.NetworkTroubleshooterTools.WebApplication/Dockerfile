#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#Troubleshooter Start
RUN apt-get update && apt-get install -y \
	wget \
	dnsutils \
	mtr \
	iputils-ping \
	traceroute \
	curl && \
	apt-get clean && \
	rm -rf /var/lib/apt/lists/*
#Troubleshooter End
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.NetworkTroubleshooterTools.WebApplication/Tgs.NetworkTroubleshooterTools.WebApplication.csproj", "Tgs.NetworkTroubleshooterTools.WebApplication/"]
RUN dotnet restore "./Tgs.NetworkTroubleshooterTools.WebApplication/Tgs.NetworkTroubleshooterTools.WebApplication.csproj"
COPY . .
WORKDIR "/src/Tgs.NetworkTroubleshooterTools.WebApplication"
RUN dotnet build "./Tgs.NetworkTroubleshooterTools.WebApplication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.NetworkTroubleshooterTools.WebApplication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.NetworkTroubleshooterTools.WebApplication.dll"]