1. Set up the project:

Create a new .NET Web API project (using .NET Framework 4.8 or .NET 7 as per your preference).
Install necessary NuGet packages (like Entity Framework for database operations, Swashbuckle for Swagger, and xUnit for testing).

```md
Create a new .NET Web API project:
Open a terminal or command prompt.
Navigate to the directory where you want to create the project.
Run the following command to create a new .NET Web API project:

For .NET 7:
dotnet new webapi -n WeatherForecastApi
```

```md
Navigate into the newly created project directory.
Install the necessary NuGet packages. Here are the commands for some of the packages you might need:

Entity Framework Core (for .NET 7 project):
dotnet add package Microsoft.EntityFrameworkCore --version 7.0.15
dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.15
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 7.0.15

Swashbuckle (for Swagger UI):
dotnet add package Swashbuckle.AspNetCore

xUnit (for testing):
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
```

2. Define the database model:

Create a Forecast class with properties for ID, Latitude, Longitude, and WeatherData.
Set up Entity Framework to use a database of your choice (like SQL Server for .NET Framework 4.8 or SQLite for .NET 7).

```md
Create a Forecast class
In your project, create a new folder named Models.
Inside the Models folder, create a new file named Forecast.cs.
In the Forecast.cs file, define a Forecast class with properties for ID, Latitude, Longitude, and WeatherData.
```

```md
Set up Entity Framework to use a database
In your project, create a new folder named Data.
Inside the Data folder, create a new file named ForecastDbContext.cs.
In the ForecastDbContext.cs file, define a ForecastDbContext class that inherits from DbContext and includes a DbSet<Forecast> property.
```

1. Create the database context:

Create a ForecastDbContext class that inherits from DbContext.

```md
In your project, you should already have a Data folder.
Inside the Data folder, you should already have a file named ForecastDbContext.cs.
In the ForecastDbContext.cs file, define a ForecastDbContext class that inherits from DbContext and includes a DbSet<Forecast> property.
```

Define a DbSet<Forecast> property.

```md
In the ForecastDbContext class, define a DbSet<Forecast> property. This represents the table of forecasts in the database. You've already done this in the previous step.
```

4. Set up the database:

In the Startup class, configure the database context in the ConfigureServices method.

```md
Open the Startup.cs file in your project.
In the ConfigureServices method, add code to configure the ForecastDbContext to use SQLite (or another database of your choice).
```

Run the database migration to create the database.

```md
Open a terminal or command prompt.
Navigate to the directory of your project.
Run the following command to create a migration:
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. Create the API controller:

Create a ForecastController with methods for adding, deleting, and retrieving forecasts.
Use the ForecastDbContext to perform database operations.

```md
In your project, create a new folder named Controllers if it doesn't exist already.
Inside the Controllers folder, create a new file named ForecastController.cs.
In the ForecastController.cs file, define a ForecastController class that inherits from ControllerBase and includes methods for adding, deleting, and retrieving forecasts.
```

This controller uses the ForecastDbContext to perform database operations. The HttpGet, HttpPost, and HttpDelete attributes are used to map HTTP verbs to controller actions.

6. Implement the weather forecast service:

Create a WeatherService class that fetches weather data from Open-Meteo.
Use this service in the ForecastController to fetch weather data when a new forecast is added.

```md
In your project, create a new folder named Services if it doesn't exist already.
Inside the Services folder, create a new file named WeatherService.cs.
In the WeatherService.cs file, define a WeatherService class with a method for fetching weather data from Open-Meteo.
```

This service uses HttpClient to send a GET request to the Open-Meteo API and returns the response as a string. You should replace the URL and parameters with the actual API endpoint and parameters.

```md
Update your Startup.cs file to register WeatherService in the dependency injection container
Update your ForecastController to use WeatherService
In this code, the Create method uses WeatherService to fetch weather data when a new forecast is added.
```

7. Set up Swagger:

In the Startup class, configure Swagger in the ConfigureServices method.
Enable the Swagger middleware in the Configure method.

8. Write unit tests:

Create a new xUnit test project.
```md
dotnet new xunit -n WeatherForecastApi.Tests
```

Add a reference to the project you're testing
```md
cd WeatherForecastApi.Tests
dotnet add reference ../WeatherForecastApi/WeatherForecastApi.csproj
```

Install necessary packages
```md
dotnet add package Moq
dotnet add package FluentAssertions
```

```md
Write tests for the ForecastController and WeatherService classes.
Create a new test class for WeatherService. Mock its dependencies and write tests for its methods.
Write tests for ForecastController: Create a new test class for ForecastController. Mock its dependencies and write tests for its methods.
```

9. Test the API:

dotnet run

Use the Swagger UI to test the API endpoints.
http://localhost:5266/swagger/index.html

Run the unit tests to ensure everything is working as expected.
dotnet test
