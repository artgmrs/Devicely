# Devicely

## Overview

**Devicely** is a modern RESTful API built with .NET 9 for device management. It allows you to register, update, list, and delete device records, supporting filtering and pagination. The project uses Entity Framework Core with PostgreSQL and is fully dockerized for easy local development and deployment.

## Architecture

### Folder Structure

```
Devicely/
├── Devicely.Api             # API layer (Controllers, Middlewares, Mappings)
├── Devicely.Application     # Application layer (Services, Interfaces)
├── Devicely.Database        # Database layer (DbContext, Entities, Migrations)
├── Devicely.Domain          # Domain layer (DTOs, Enums, Constants)
├── Devicely.Test            # Automated tests (unit)
├── compose.yaml             # Docker Compose configuration
└── readme.md                # Project documentation
```

## Technologies

- [.NET 9](https://dotnet.microsoft.com/)
- [Entity Framework Core 9](https://learn.microsoft.com/en-us/ef/core/)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker & Docker Compose](https://www.docker.com/)
- [Swashbuckle (Swagger)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [xUnit](https://xunit.net/)
- [AutoFixture](https://github.com/AutoFixture/AutoFixture)

## Expected HTTP Status Codes

- **GET /api/devices**  
  - `200 OK` with a paginated list of devices (even if empty).
- **GET /api/devices/{id}**  
  - `200 OK` with the device if found.
  - `404 Not Found` if the device does not exist.
- **POST /api/devices**  
  - `201 Created` with the created device and its location.
  - `400 Bad Request` if the request data is invalid.
- **PUT /api/devices/{id}**  
  - `200 OK` with the updated device if successful.
  - `404 Not Found` if the device does not exist.
  - `400 Bad Request` if the request data is invalid.
- **DELETE /api/devices/{id}**  
  - `204 No Content` if the device was deleted.
  - `404 Not Found` if the device does not exist.
- **GLOBAL**
  - `500 Internal Server Error` if any error occurred (handled by middleware).

## How to Run

### Local Development

1. **Configure the Database Connection**

   Edit `Devicely.Api/appsettings.Development.json` if needed:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Username=dev;Password=dev;Database=devices_db"
   }
   ```

2. **Start PostgreSQL Locally**  
   You can use Docker or a local installation.

3. **Run the API**
   ```bash
   dotnet run --project Devicely.Api
   ```

4. **Access Swagger UI:**  
   http://localhost:5098/swagger (or the port configured in `launchSettings.json`).

### Running with Docker

1. **Build and Start All Services**
   ```bash
   docker-compose up --build
   ```

2. **API will be available at:**  
   http://localhost:8080/swagger

3. **PostgreSQL will be available at** `localhost:5432` (user: dev, password: dev, db: devices_db).

4. **If you need to rebuild the whole app, use:**
    ```bash
    docker-compose down -v --remove-orphans
    docker-compose build --no-cache
    docker-compose up
    ```

### Migrations

#### Add migration
1.Run command in root folder
```
dotnet ef migrations add InitialMigration --project Devicely.Database --startup-project Devicely.Api
```

#### Apply migration
1.Run command in root folder
```
dotnet ef database update --project Devicely.Database --startup-project Devicely.Api
```

## Testing

The tests use xUnit, AutoFixture, and Moq, and focus on the Service layer, where all business logic resides.
Uses an in-memory database seeded to simulate a real database with EF Core.
With this modern approach using these libraries, tests use the `[Theory]` attribute because they receive mock requests via their parameters.

### Showcasing the unit test approach:

An initial configuration is made by creating an annotation called `AutoDomainData` in `AutoDomainDataAttribute.cs`:
```csharp
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Devicely.Test.Configuration;

public class AutoDomainDataAttribute : AutoDataAttribute
{
    public AutoDomainDataAttribute()
    : base(() =>
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        
        // Configuration of standard mocks such as EF Core
        
        return fixture;
    })
    {}
}
```

With this, test creation is much cleaner and easier, removing the need to manually set up every single system dependency.
I also chose not to explicitly use the 'AAA' (Arrange, Act, Assert) pattern, as it's implicit where those blocks are.

Here is an example of a test using this approach:
```csharp
[Theory, AutoDomainData]
public void GetAllDevices_ShouldReturnAllDevices(DeviceService sut)
{
    var result = sut.GetAllDevices(brand: null, state: null, PaginationConstants.DefaultPageSize, PaginationConstants.DefaultPageNumber);
    
    Assert.NotNull(result);
    Assert.All(result, device => Assert.True(!device.IsDeleted));
}
```

### How to run tests
Automated tests can be run with:
```bash
dotnet test
```

## Notes

- My focus was to build a simple but efficient architecture that provides good separation of responsibilities, code reuse, clear understanding, and makes future maintenance and improvements easier for other developers.
- I tried to follow concepts such as Clean Code, SOLID, DRY, and KISS.
- I chose not to use AutoMapper and instead created the mappings manually, partly due to recent changes in the library and also to experiment with a hands-on approach. This experience really paid off, as I have much more control (even if it means writing a few more lines of code), and all the mapping code is mine. :)
- I started using Guid for the Device's Id but switched to int to improve pagination performance (ordering by int id is faster than by createdAt datetime, both due to data type and because the PK is already indexed by default).
- All async methods have the suffix 'Async'.
- Swagger is not exposed in the production environment, only in development and staging.
- Implemented [Early Return](https://medium.com/swlh/return-early-pattern-3d18a41bba8) wherever possible!
- All migrations are applied automatically on startup.
- Exception handling is centralized via middleware for clean controller code.

## Improvements made
- Docker uses a readiness script (`wait-for-postgres.sh`) to check if the database is ready before applying EF migrations.
- The API supports filtering and pagination out-of-the-box.
    - Added pagination to the get endpoint. It's optional and has default values of `pageSize = 10` and `pageNumber = 1`.
- I opted for 'soft delete' instead of actually removing data from the database. To achieve this, I created an `isDeleted` property in the Device entity, which is updated by the delete endpoint.
    - Because of that, in the getAll endpoint I return all possible Device states, but only records where `isDeleted` is `false`.
- I created an `updatedAt` property to track updates to the Device. The datetime is updated by the PUT endpoint.

## Future improvements

- Add authentication and authorization.
- In production, remove sensitive variables such as connection strings from the repository and move them to environment variables managed by your CI/CD provider.
- If more complex queries are needed, consider a hybrid approach with Entity Framework and Dapper, or introduce a repository layer.
- Create new table to store the device states and add it as FK at Device's table.
- Add an AppService in the API layer to act as an interface between the Controller and Service layers.
- Add versioning to the API (e.g., v1 folders).
- Improve error handling with custom exceptions and validations (possibly with FluentValidation).
- Improve database data types to ensure efficiency.
- Make the get endpoint filters case-insensitive as well.
- Respecting the SRP from SOLID, `DeviceService.cs` still has too many responsibilities. It would be good to move validations to another class.
