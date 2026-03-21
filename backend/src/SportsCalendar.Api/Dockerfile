# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj & restore
COPY ["SportsCalendar.Api/SportsCalendar.Api.csproj", "SportsCalendar.Api/"]
COPY ["SportsCalendar.Application/SportsCalendar.Application.csproj", "SportsCalendar.Application/"]
COPY ["SportsCalendar.Domain/SportsCalendar.Domain.csproj", "SportsCalendar.Domain/"]
COPY ["SportsCalendar.Infrastructure/SportsCalendar.Infrastructure.csproj", "SportsCalendar.Infrastructure/"]
COPY ["SportsCalendar.Contracts/SportsCalendar.Contracts.csproj", "SportsCalendar.Contracts/"]

RUN dotnet restore "SportsCalendar.Api/SportsCalendar.Api.csproj"

# copy everything and publish
COPY . .
WORKDIR "/src/SportsCalendar.Api"
RUN dotnet build "SportsCalendar.Api.csproj" -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "SportsCalendar.Api.dll"]