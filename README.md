## Overview

The Sports Calendar API allows users to manage sport events data. 

List of currently available endpoitns:

- `GET` `/api/events/{{eventId}}`: Fetches event & all child entity details for a single event.
- `GET` `/api/events?page=&pageSize=`: Fetches multiple event details. Provides paginated responses with total count of events meeting the filter requirements.

  Available filters:
  - `startDate`
  - `endDate`
  - `sportId`
  
  All query parameters are optional.
- `POST` `/api/events`: Creates an event with child entities (specify in `new{EntityName}`) or links existing entities (specify entity id in `existing{EntityId}`).

  If both provided **prioritizes existing entities**.

---

## Key Features

- **Atomic Event Creation:** Create an event, its teams, stadium, and competition in a single transactional request.
- **Clean Architecture:** Strict separation of concerns between Api, Contracts, Domain, Application, and Infrastructure layers.
- **Result Tracking:** Detailed match results including period scores and specific incidents (Goals, Yellow Cards, etc.).
- **Docker Integration:** Configured Docker & Docker-Compose support (api + database) for easier deployments.
- **Tests:** Created crucial tests for each implented endpoint.
- **Health Checks**:

  The API includes a diagnostic endpoint to verify system readiness:

  **Endpoint:** `/health`
  checks: 
  - self: API responsiveness.
  - sqlserver: Database connectivity.

---

## Tech Stack

- **Framework:** .NET 10.0

- **Database:** SQL Server 

- **ORM:** Dapper
 
- **Patterns:** CQRS with MediatR, Unit of Work, Repository Pattern

- **Error Handling:** ErrorOr, ProblemDetails

- **Mapping:** Mapster

- **Validaton:** FluentValidation

- **Deployment:** Docker

- **Testing:** Postman

---

## Setup & Installation

### Prerequisites

- .NET 10 SDK

- SQL Server

- Postman (optional)

- Docker and Docker-Compose (optional)


### Installation

### Clone the Repository
```
git clone https://github.com/skmkqw/sportradar-be-coding-excercise.git
cd SportsCalendar
```

### Configure Database

- Start the Database (if running without Docker).
Ensure you have a working SQL Server instance.

- Connect to your database using preferred tools and execute: `init-db.sql` to initialize the database and `seed-db.sql` to populate the database with seed data.

- Create a `.env` file (use `.env.example` for reference). Update the connection string.

### Run the Application 
Local:
```
cd backend/src
dotnet run --project SportsCalendar.Api
```

Docker:
```
docker compose up -d --build
```

The API will typically be available at http://localhost:5138 (local) or https://localhost:5000 (Docker).

### Test the Application
- Import colletions and environments from `/backend/tests/postman` to your Postman client.
- Assign values to collection/environment variables.
- Run collections or individual requests

---

## Architectural Decisions & Assumptions

**1. "Resolve or Create" Pattern**

  In the AddEvent command, I chose a Resolution Result pattern. The API allows client to provide an existingId or a newEntity object for Teams, Stadiums, Stages and Competitions.
  
  Decision: Entities are resolved before starting a database transaction. This minimizes lock contention and prevents partial data writes if a validation fails halfway through.

**2. The "Anchor" SportId**

  Decision: The Competition acts as the source of truth for the event's sport.

  Assumption: Every team assigned to an event must share the same SportId as the competition. If a user tries to create an event with mismatched sports, the API returns a Validation error before touching the database.

**3. Dapper + Unit of Work**
  
  Decision: I used Dapper for its speed but wrapped it in a custom IUnitOfWork to manage IDbTransaction.
  
  Assumption: All repositories must explicitly receive the IDbTransaction object to ensure they participate in the same atomic operation initiated by the Command Handler.

**4. Result Normalization**

  Assumption: Total scores for an event are derived from the sum of PeriodScores. While the database stores the total for quick lookups, the domain logic ensures these are kept in sync during creation.
