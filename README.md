# Howestprime Backoffice

A lightweight cinema backoffice built with .NET and Blazor Server. It helps staff manage the movie catalog, inspect movie details, and plan screening schedules for rooms across the month.

## Overview

This project contains a front-facing admin application for the Howestprime movie platform:

- Register and update movies in the catalog
- Browse available movies and review details
- Schedule movie events in rooms
- View a monthly planning calendar
- Validate and handle API-driven business rules from the mock backend

## Tech stack

- ASP.NET Core 10
- Blazor Server
- C#
- Typed API client project for movie operations
- Local mock API server for development and demo scenarios

## Repository structure

- `Howestprime.Backoffice/` - Blazor backoffice application
- `Howestprime.Movies.ApiClient/` - reusable API client and request/response models
- `Howestprime.Movies.MockServer/` - local mock API that simulates the movie backend
- `Howestprime.Backoffice.slnx` - solution entry point

## Prerequisites

Before running the project, make sure you have:

- .NET 10 SDK installed
- A terminal or shell environment
- A browser to open the app

## Run locally

### 1. Restore dependencies

```bash
dotnet restore
```

### 2. Start the mock API server

```bash
dotnet run --project Howestprime.Movies.MockServer
```

The mock server listens on:

- `http://localhost:8000`

### 3. Start the backoffice app

```bash
dotnet run --project Howestprime.Backoffice
```

The app starts with the default development settings and is typically available at:

- `https://localhost:7155`
- or `http://localhost:5243`

## Main features

### Movie catalog

- Add new movies to the catalogue
- View movie metadata such as title, year, genres, actors, duration, and age rating
- Preview poster URLs and details

### Planning

- Schedule screenings for different rooms
- Check for conflicts in the room schedule
- Inspect a month-based calendar layout for movie events

## Development notes

The backoffice app configures the movie API client using the section named `MoviesApi` in `appsettings.json`:

```json
{
  "MoviesApi": {
    "BaseUrl": "http://localhost:8000/",
    "TimeoutSeconds": 30
  }
}
```

This means the app expects the mock API to be running before opening the application in a browser.

## Typical workflow

1. Start the mock server.
2. Start the Blazor app.
3. Open the backoffice in your browser.
4. Register or browse movies.
5. Schedule showings in the planning view.

## License

This project is for educational and demo use within the Howestprime course context.
