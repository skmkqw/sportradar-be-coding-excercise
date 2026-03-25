# Sports Calendar

## Overview

Sports Calendar is a full-stack app for browsing, creating, and inspecting sports events.

The project currently includes:

- a SQL Server database
- a .NET backend API
- a Next.js frontend
- Docker Compose orchestration for all three services

## Current App State

### Frontend

The Next.js app lives in `frontend/sports-calendar` and currently provides:

- a month-based calendar view
- month and year navigation
- event cards grouped by venue date
- clickable event detail pages
- a responsive event details screen with score, teams, venue, period scores, and incidents
- a responsive "Create Event" page
- a "Populate with placeholder data" helper button for fast form testing
- an API health indicator in the header

### Backend API

The API currently exposes:

- `GET /health`
  Checks API and database readiness.

- `GET /api/events/{eventId}`
  Returns a single event with nested child entities and result details.

- `GET /api/events?page=&pageSize=&startDate=&endDate=&sportId=`
  Returns paginated events. All query parameters are optional.

- `POST /api/events`
  Creates a new event.
  Existing child entity IDs take precedence over any provided `new...` payloads.

## Tech Stack

- Frontend: Next.js 16, React 19, TypeScript, Tailwind CSS
- Backend: .NET 10, ASP.NET Core, MediatR, FluentValidation, Mapster
- Data access: Dapper
- Database: SQL Server 2022
- Containerization: Docker, Docker Compose

## Repository Layout

```text
.
├── backend/
│   ├── src/
│   └── tests/
├── database/
├── frontend/
│   └── sports-calendar/
├── docker-compose.yml
├── .env.example
└── README.md
```

## Environment Variables

Copy `.env.example` to `.env` in the repository root before running Docker Compose.

Current root environment variables:

- `DB_NAME`
- `DB_USER`
- `DB_PASSWORD`
- `DB_PORT`
- `CONNECTION_STRING`
- `NEXT_PUBLIC_API_BASE_URL`
- `INTERNAL_API_BASE_URL`

What they are used for:

- `DB_*` values configure SQL Server and the backend connection string
- `NEXT_PUBLIC_API_BASE_URL` is used by the browser-side frontend code
- `INTERNAL_API_BASE_URL` is used by server-side frontend code inside Docker

## Running With Docker

From the repository root:

```bash
docker compose up -d --build
```

This starts:

- `sqlserver`
- `backend`
- `frontend`

Default local URLs:

- Frontend: `http://localhost:3000`
- Backend API: `http://localhost:5000`
- Health check: `http://localhost:5000/health`

## Running Locally Without Docker

### Backend

Requirements:

- .NET 10 SDK
- SQL Server

Start the backend from the backend source folder:

```bash
cd backend/src
dotnet run --project SportsCalendar.Api
```

### Frontend

Requirements:

- Node.js 22+

Start the frontend:

```bash
cd frontend/sports-calendar
npm install
npm run dev
```

The frontend expects the API at `http://localhost:5000` by default.

If needed, you can override that with:

```bash
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000
INTERNAL_API_BASE_URL=http://localhost:5000
```

## Frontend Notes

The frontend uses two API base URLs:

- `NEXT_PUBLIC_API_BASE_URL`
  Used in browser/client code.

- `INTERNAL_API_BASE_URL`
  Used in server actions and server-rendered data fetching.

In Docker Compose the internal API URL must point to the backend service name:

```text
http://backend:8080
```

## Event Creation Payload Rules

For child entities:

- Home team: required
- Away team: required
- Competition: required
- Stadium: optional
- Stage: optional
- Result: optional

For each child entity, either:

- provide an existing ID, or
- provide a `new...` object

If both are sent, the existing ID is used and the new object is ignored.

## Testing

### Frontend

From `frontend/sports-calendar`:

```bash
npm run lint
```

### Backend

Postman collections are available under `backend/tests/postman`.

## Architectural Notes

### Resolve-or-create child entities

The API accepts either existing IDs or full payloads for new related entities. This keeps event creation flexible while preserving referential integrity.

### Competition as sport anchor

The competition acts as the source of truth for the sport, and related teams are expected to match that sport.

### Atomic creation

Event creation, related entities, and result data are handled transactionally in the backend.
