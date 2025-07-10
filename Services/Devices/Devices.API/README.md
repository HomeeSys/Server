# Devices Service

The **Devices Service** manages all device-related functionality within the system, including registration, modification, deletion, and data retrieval. The service's API is exposed via the `Devices.API` project.

---

## 🛠️ Technologies Used

This service leverages the following technologies:

* **Entity Framework** – Object-relational mapping (ORM) tool.
* **MS SQL Server** – Relational database system.
* **MediatR** – Implements CQRS-like design using the mediator pattern.
* **Mapster** – Maps database entities to Data Transfer Objects (DTOs).
* **Carter** – Lightweight, minimal API endpoints.
* **FluentValidation** – Input validation library.
* **Docker** – Containerization and deployment.

---

## 📂 Project Structure

The Devices Service is organized into five main projects:

| Project                  | Description                                                                                              |
| ------------------------ | -------------------------------------------------------------------------------------------------------- |
| `Devices.API`            | Exposes endpoints and serves as the entry point for the service. Wraps all other layers.                 |
| `Devices.Application`    | Contains application logic, DTOs, validators, and endpoint definitions. Not exposed directly to clients. |
| `Devices.Infrastructure` | Handles database communication. Any changes to DB interactions occur here.                               |
| `Devices.Domain`         | Contains core domain models and business logic definitions.                                              |
| `Devices.DataAccess`     | Holds MS SQL scripts for development and testing purposes.                                               |

> ⚠️ **Note:** `Devices.API` is currently configured for automatic migrations. Each time the app is run, the database schema will be updated/remigrated.

---

## 🔍 API Endpoints

> ℹ️ **Note:** The following are key endpoints used by the system.

| Method | URI                          | Description                                             | Response                |
| ------ | ---------------------------- | ------------------------------------------------------- | ----------------------- |
| GET    | `/devices/all`               | Retrieve all devices.                                   | List of device details. |
| GET    | `/devices/{id}`              | Retrieve a device by its internal ID.                   | Device data.            |
| GET    | `/devices/devicenumber/{id}` | Retrieve a device by its unique device number.          | Device data.            |
| POST   | `/devices`                   | Register a new device. Used to mark a device as active. | Device data.            |
| PUT    | `/devices`                   | Update an existing device.                              | Device data.            |
| DELETE | `/devices/{id}`              | Remove a device by its ID.                              | Device data.            |

---

## 📊 Database Entities

The service uses four main database tables, defined in `Devices.API`:

### Devices

> ℹ️ **Main table for device-related information.**

| Type       | Column                    | Description                                                            |
| ---------- | ------------------------- | ---------------------------------------------------------------------- |
| `Int`      | ID                        | Primary key.                                                           |
| `Guid`     | DeviceNumber              | Unique device identifier (used instead of ID for external references). |
| `DateTime` | RegisterDate              | Timestamp of when the device was registered.                           |
| `String`   | Description               | Device details (e.g., model, peripherals).                             |
| `FK`       | LocationID                | Foreign key to the `Locations` table.                                  |
| `FK`       | TimestampsConfigurationID | Foreign key to the `TimestampsConfigurations` table.                   |
| `FK`       | StatusID                  | Foreign key to the `Statuses` table.                                   |

### Statuses

> ℹ️ **Represents the status of each device.**

Examples:

* `Registered`: Device added but not yet used.
* `Active`: Currently measuring.
* `Removed`: Deleted or deactivated.

| Type     | Column |
| -------- | ------ |
| `Int`    | ID     |
| `String` | Name   |

### Locations

> ℹ️ **Represents physical locations where devices are placed.**

Examples:

* Kitchen
* Living Room
* Attic

| Type     | Column |
| -------- | ------ |
| `Int`    | ID     |
| `String` | Type   |

### TimestampsConfigurations

> ℹ️ **Defines how often a device should send measurements using CRON format.**

| Type     | Column |
| -------- | ------ |
| `Int`    | ID     |
| `String` | Cron   |

---

## 📚 MediatR

**MediatR** is a lightweight .NET library designed to implement the **Mediator** pattern easily. It helps structure request/response logic while supporting custom behaviors like logging and validation through its pipeline.

### Request/Response Example

A basic example of defining a request and its response:

```csharp
public record GetAllDevicesCommand() : IRequest<GetAllDevicesResponse>;

public record GetAllDevicesResponse(IEnumerable<Device> Devices);
```

Handler Example
This handler processes the request, assuming validation and other behaviors have passed:

```cs
public class GetAllDevicesHandler(DevicesDBContext context) : IRequestHandler<GetAllDevicesCommand, GetAllDevicesResponse>
{
    public async Task<GetAllDevicesResponse> Handle(GetAllDevicesCommand request, CancellationToken cancellationToken)
    {
        var result = await context.Devices.ToListAsync();

        return new GetAllDevicesResponse(result);
    }
}
```

This pattern promotes a clean separation of concerns, allowing each part of the system (validation, data access, business logic) to remain modular and testable.

---

## Mapster
This is mapping library to allows for easy mapping between `DTO` and `Model` instance.

How to use it:
``` cs
var result = await sender.Send(new GetAllDevicesCommand());

var response = result.Adapt<GetAllDevicesResponse>();
```

---

## Carter
This library allow for easy mapping for endpoints.

> ⚠ **Important** Remember to make class that derives from `ICarterModule` as `public` because otherwise `Carter` will not be able to scan assembly properly and this will not create enpoint, thus not allowing client to talk to it.

Example:
``` cs
public class CreateDeviceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/devices/", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllDevicesCommand());

            var response = result.Adapt<GetAllDevicesResponse>();

            return Results.Ok(response);
        });
    }
}
```

---

## FluentValidator
Allows to define an easy way to validate DTOs send from client.

Example:
``` cs
public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceCommandValidator()
    {
            
    }
}
```
