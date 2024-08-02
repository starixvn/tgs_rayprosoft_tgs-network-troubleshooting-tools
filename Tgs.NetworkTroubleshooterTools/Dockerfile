#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
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
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.NetworkTroubleshooterTools/Tgs.NetworkTroubleshooterTools.csproj", "Tgs.NetworkTroubleshooterTools/"]
COPY ["Tgs.NetworkTroubleshooterTools.Core/Tgs.NetworkTroubleshooterTools.Core.csproj", "Tgs.NetworkTroubleshooterTools.Core/"]
RUN dotnet restore "./Tgs.NetworkTroubleshooterTools/Tgs.NetworkTroubleshooterTools.csproj"
COPY . .
WORKDIR "/src/Tgs.NetworkTroubleshooterTools"
RUN dotnet build "./Tgs.NetworkTroubleshooterTools.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.NetworkTroubleshooterTools.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.NetworkTroubleshooterTools.dll"]