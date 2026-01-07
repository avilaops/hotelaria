FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Hotelaria.csproj", "./"]
RUN dotnet restore "Hotelaria.csproj"
COPY . .
RUN dotnet build "Hotelaria.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hotelaria.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y \
    curl \
    && rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE $PORT
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:$PORT/ || exit 1
ENTRYPOINT ["dotnet", "Hotelaria.dll"]
