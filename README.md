# Mikl + Duplo: Weather Forecast API

This API allows you to retrieve and store weather forecasts provided by open-meteo.com.

## API Routes

- **POST `/Forecast`**
  - Pulls and stores forecast data by the input latitude and longitude from the request body and returns the forecast.
  ```bash
  curl -X POST -H "Content-Type: application/json" -d '{"latitude": "50.8503", "longitude": "4.3517"}' http://localhost:5266/Forecast
  ```

- **GET `/Forecast`**
  - Returns all stored weather forecasts.
  ```bash
  curl -X GET http://localhost:5266/Forecast
  ```

- **GET `/Forecast/{id}`**
  - Returns a specific weather forecast identified by `{id}`.
  ```bash
  curl -X GET http://localhost:5266/Forecast/1
  ```

- **DELETE `/Forecast/{id}`**
  - Deletes the weather forecast identified by `{id}`.
  ```bash
  curl -X DELETE http://localhost:5266/Forecast/1
  ```

- **PUT `/Forecast/{id}/update`**
  - Updates the weather forecast identified by `{id}` with fresh data.
  ```bash
  curl -X PUT -H "Content-Type: application/json" -d '{"latitude": "50.8503", "longitude": "4.3517"}' http://localhost:5266/Forecast/1/update
  ```

- **GET `/Forecast/{id}/refresh`**
  - Fetches the latest weather data for the forecast identified by `{id}`, updates the stored forecast, and returns the updated forecast.
  ```bash
  curl -X GET http://localhost:5266/Forecast/1/refresh
  ```

- **GET `/Forecast/history`**
  - Returns a hyperlinked HTML page with the history of all previously submitted latitudes and longitudes. Selecting a link calls `/Forecast/{id}/refresh`.
  ```bash
  curl -X GET http://localhost:5266/Forecast/history
  ```

## Installation and Setup

1. Install .NET 7
2. Clone into this repository
3. `dotnet restore` to install dependencies
4. `cd WeatherForecastApi && dotnet tool restore` to enable ef migrations
5. `dotnet ef database update` to create the database
6. `dotnet run --project WeatherForecastApi` to start the server

The API will be available locally at http://localhost:5266/

The interactive history of all previously submitted latitudes and longitudes can be viewed in a browser at http://localhost:5266/Forecast/history

## Testing

1. Run `dotnet test` to execute all tests

API testing is available while the application is running using the Swagger UI at http://localhost:5266/swagger/index.html.

A GitHub Actions workflow is configured to run unit tests on every push and pull request.

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
- Rate limiting
- Debug logging
