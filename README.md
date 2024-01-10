# Mikl + Duplo: Weather Forecast API

This API allows you to retrieve and store weather forecasts provided by open-meteo.com.

## API Routes

- **POST `/Forecast`**
  - Pulls and stores forecast data by the input latitude and longitude from the request body and returns the forecast.
- **GET `/Forecast`**
  - Returns all stored weather forecasts.
- **GET `/Forecast/{id}`**
  - Returns a specific weather forecast identified by `{id}`.
- **DELETE `/Forecast/{id}`**
  - Deletes the weather forecast identified by `{id}`.
- **PUT `/Forecast/{id}/update`**
  - Updates the weather forecast identified by `{id}` with fresh data.
- **GET `/Forecast/{id}/refresh`**
  - Fetches the latest weather data for the forecast identified by `{id}`, updates the stored forecast, and returns the updated forecast.
- **GET `/Forecast/history`**
  - Returns a hyperlinked HTML page with the history of all previously submitted latitudes and longitudes. Selecting a link calls `/Forecast/{id}/refresh`.

## Installation and Setup

1. Install .NET 7
2. Clone into this repository
3. Run `dotnet restore` to install dependencies
4. Run `dotnet run --project WeatherForecastApi` to start the server

The API will be available locally at http://localhost:5266/

Testing is available using the Swagger UI at http://localhost:5266/swagger/index.html.

The interactive history of all previously submitted latitudes and longitudes can be viewed in a browser at http://localhost:5266/Forecast/history

## Testing

1. Run `dotnet test` to execute all tests

A GitHub Actions workflow is configured to run tests on every push and pull request.

## Technology

- C#
- .NET 7
- SQLite
- Swagger
- xunit
- Moq
- MVC
- Razor Pages

## Future Improvements

- Authentication and authorization
- Enhanced error handling
- Further separation of concerns
- Increase test coverage
- Rate limiting
- Debug logging
