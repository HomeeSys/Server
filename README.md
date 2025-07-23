# `Homee system` - Backend

<img src="Assets/homeelogo.png" style="float: left;
    margin-right: 40px;
    margin-bottom: 20px;
    width: 300px;" alt="Image">

The `Homee System` backend is built with `.NET 8` and `ASP.NET Core`, designed as a distributed architecture composed of multiple `microservices` that operate independently yet integrate seamlessly to form a reliable and scalable platform. Its primary purpose is to serve as a central backend for managing devices and sensors within a single facility, enabling `device registration`, `real-time measurement data collection`, and `automated report generation`. 

System exposes both `RESTful` endpoints and `gRPC` connections to facilitate efficient client communication and service-to-service interactions. To ensure flexibility and scalability, the backend is fully `containerized` and optimized for deployment in orchestrated environments such as `Azure Container Apps` or local `Docker-compose`, while a Docker Compose setup allows developers to run the entire stack locally without requiring any external dependencies. 

This repository focuses on the `backend architecture` of the `Homee System`, emphasizing clean separation of concerns, high maintainability, and an `event-driven approach` to data processing.

## `System's components`

System is designed as a collection of independent services, each with a clearly defined responsibility:
- `Device Management` – Handles the full lifecycle of devices, including adding, updating, and removing them from the system.
- `Measurement Collection` – Records and stores data captured by devices for later processing and analysis.
- `Report Generation` – Creates comprehensive reports that summarize collected data and make it accessible for users.
- `Intelligent Assistant` – Analyzes measurement data, identifies key insights, and provides summaries that enhance the reports.

Additional supporting components include:
- `Message Queue` – Enables asynchronous communication between services, ensuring reliable data exchange.
- `Gateway Service` – Acts as a single entry point for all requests and routes them to the correct service within the system.

## `Architecture`

Backend architecture is illustrated in the following diagram:

![image](Assets/architecture-top-most.png)

At the top level, the system is composed of four core microservices (`Devices`, `Measurements`, `Reports`, and `AiAssistant`), supported by two key infrastructure components:

`API Gateway` – Serves as a single entry point for all external requests and routes them to the appropriate microservices. It is implemented using the `YARP` library, which offers advanced features such as `connection throttling` and `authentication`. In this implementation, `YARP` is primarily used for `traffic redirection` and consolidating all requests into one gateway.

`Message Queue` – Handles asynchronous messaging between services, implemented using `RabbitMQ` and the `MassTransit` library. It is one of the two communication methods between services and is primarily used to store and distribute messages related to report generation.

### `Microservices`

All microservices run inside separate `Docker containers` and are `orchestrated` together using `Docker Compose`, providing a fully containerized and self-contained backend environment.

- `Devices` - This microservice manages all operations related to `registering`, `updating`, and `removing devices` within the system. It uses `Entity Framework` as the `ORM` for interacting with a `MS SQL Server database`. This service also provides a `gRPC server` for sharing device information with other services. To maintain a clean and modular architecture, it leverages several libraries:
    - `FluentValidation` – Validating all incoming data.
    - `Carter` – Building minimal API endpoints.
    - `MediatR` – Implements the mediator pattern, enabling clean separation of concerns and streamlined request handling.
    - `Mapster` – Mapping between DTOs and internal domain models.

- `Measurements` - Microservice focuses on storing data captured by devices. Since the structure of measurement data can vary, a `NoSQL` database was chosen. It uses `Azure Cosmos DB`, which offers low latency, high performance, and generous free-tier storage. Like the Devices service, it also provides a `gRPC server` to share measurement data with clients. It features endpoints for registering and retrieving measurements. For report generation, this service aggregates data by calling the gRPC endpoints of Devices and other services, processes the data, and packages it for the AiAssistant service to produce descriptive insights.

- `Reports` - Generates data summaries based on the information stored in Measurements. Currently, it supports generating daily reports, which are exported as `PDF` files and stored in `Azure Blob Storage`. This design ensures that reports remain accessible even when the service is offline.

- `AiAssistant` - Uses an embedded `Ollama` container running `Meta’s LLaMa` model to analyze measurement data. It exposes a `gRPC server` that receives data packets, processes them, and generates descriptive summaries. These AI-generated summaries are then included in the final reports produced by the Reports service.

All microservices running in single `Docker-compose`:
![image](Assets/containers.png)

---

# Devices

## Database schema

The `Devices` service manages detailed information about each device, including unique identifiers, registration dates, descriptions, and links to their locations, statuses, and measurement schedules. Devices are assigned statuses like registered, active, or removed to indicate their current state. Measurement frequencies are configured using CRON expressions stored in a separate configuration table, allowing flexible scheduling of device data collection. Together, these tables organize device data, operational states, and timing configurations to support effective device management.


![image](Assets/dbschema.png)

## Endpoints

``Devices`` microservice feature couple of endpoints. They are privided in table below.

| Method | URI                                | Description | Response |
|--------|------------------------------------|-------------|----------|
| GET    | `/devices/all`                   | Retrieve all devices available in the system. | List of full device details. |
| GET    | `/devices/{ID}`                 | Retrieve a device by its ID. | Device data. |
| GET    | `/devices/devicenumber/{DeviceNumber}`   | Retrieve a device by its device number. | Device data. |
| POST   | `/devices/`                     | Register a new device. This endpoint lets the server know that the device is active. Whether restarted or resumed, it must send data here. | Device data. |
| PUT    | `/devices/{DeviceNumber}`                     | Update an existing device. | Device data. |
| DELETE | `/devices/{ID}`                | Remove a device by its ID. | Device data. |
| GET | `/health`                | Check health of this microservice | Health status for this service and servies that relay on it. |

## Showcase

This demo shows how to interact with `Devices` service via it's endpoints by using `Postman`.

![image](Assets/DevicesShowcase.gif)

---

# Measurements

# Raports

# AiAssistant

# Message queue

# API Gateway