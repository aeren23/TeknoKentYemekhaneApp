# TeknoKentYemekhaneApp

A modern cafeteria management application built with Blazor and .NET Aspire, demonstrating clean architecture principles and modern cloud-native development practices.

## 🎯 Project Overview

This project showcases a production-ready cafeteria application that implements modern software development practices including **Blazor**, **.NET Aspire**, **Docker**, **JWT Authentication**, **CQRS**, and **Onion Architecture**. The application is designed to be scalable, maintainable, and cloud-ready.

## 🏗️ Architecture

### Onion Architecture

The project follows **Onion Architecture** principles, ensuring separation of concerns and high testability:

```
TeknoKentYemekhaneApp/
│
├── YemekhaneApp.Domain/           # Core Layer
│   ├── Entities/                  # Business entities
│   └── BaseEntities/              # Base entity abstractions
│
├── YemekhaneApp.Application/      # Application Layer
│   ├── CQRS/                      # Commands and Queries (CQRS pattern)
│   ├── DTOs/                      # Data Transfer Objects
│   ├── Interfaces/                # Application contracts
│   ├── Mappings/                  # AutoMapper profiles
│   ├── Services/                  # Application services
│   └── Wrappers/                  # Response wrappers
│
├── YemekhaneApp.Persistence/      # Infrastructure Layer
│   ├── Context/                   # Database context
│   ├── Repositories/              # Repository implementations
│   ├── Migrations/                # EF Core migrations
│   └── UnitOfWork.cs              # Unit of Work pattern
│
├── Backend/API/YemekhaneApp.Api/  # API Layer
│   ├── Controllers/               # REST API endpoints
│   └── Dockerfile                 # API containerization
│
├── UI/YemekhaneApp.Frontend/      # Presentation Layer
│   ├── Components/                # Blazor components
│   ├── Services/                  # Frontend services
│   └── Dockerfile                 # Frontend containerization
│
├── AspireApp/                     # .NET Aspire Orchestration
│   └── AspireApp.AppHost/         # Aspire app host configuration
│
├── docker-compose.yml             # Multi-container orchestration
└── docker-compose.server.yml      # Production deployment
```

### Layer Responsibilities

- **Domain Layer**: Contains business entities and core business logic, independent of any external dependencies
- **Application Layer**: Implements CQRS patterns, business use cases, and application services
- **Persistence Layer**: Handles data access using Entity Framework Core and implements Repository pattern
- **API Layer**: Exposes RESTful endpoints with JWT authentication
- **Presentation Layer**: Blazor Server UI with interactive components

## 🚀 Technologies & Tools

### Core Technologies
- **.NET 8**: Latest .NET framework
- **Blazor Server**: Interactive server-side UI framework
- **Entity Framework Core**: ORM for database operations
- **SQL Server 2022**: Relational database

### Architecture Patterns
- **Onion Architecture**: Dependency inversion and separation of concerns
- **CQRS**: Command Query Responsibility Segregation with MediatR
- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management

### Cloud-Native & DevOps
- **.NET Aspire**: Cloud-ready app orchestration and service discovery
- **Docker**: Containerization for API, Frontend, and Database
- **Docker Compose**: Multi-container orchestration
- **DataProtection**: Distributed key management for scalability

### Security
- **JWT (JSON Web Tokens)**: Stateless authentication and authorization
- **Custom Authentication Handler**: Secure cookie-based authentication
- **CORS**: Cross-origin resource sharing configuration

### Development Tools
- **AutoMapper**: Object-to-object mapping
- **MediatR**: Mediator pattern implementation for CQRS
- **Swagger/OpenAPI**: API documentation and testing

## 🔧 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or higher
- [Docker Desktop](https://www.docker.com/get-started)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (optional)

### Configuration

1. **Clone the repository**
   ```bash
   git clone https://github.com/aeren23/TeknoKentYemekhaneApp.git
   cd TeknoKentYemekhaneApp
   ```

2. **Configure environment variables**
   
   Create a `.env` file in the root directory:
   ```env
   CONNECTIONSTRINGS__YEMEKHANEDB=Server=api-db;Database=YemekhaneDb;User Id=sa;Password=YourStrong@Password;TrustServerCertificate=True;
   MSSQL_SA_PASSWORD=YourStrong@Password
   ```

### Running with Docker Compose (Recommended)

This is the easiest way to run the entire application stack:

```bash
docker-compose up --build
```

**Services will be available at:**
- **Frontend**: http://localhost:10000
- **API**: http://localhost:5110
- **API Swagger**: http://localhost:5110/swagger

### Running with .NET Aspire

.NET Aspire provides advanced orchestration with service discovery and telemetry:

```bash
cd AspireApp/AspireApp.AppHost
dotnet run
```

The Aspire dashboard will open automatically, providing:
- Service health monitoring
- Distributed tracing
- Logs aggregation
- Metrics visualization

### Running in Development Mode

1. **Start the database**
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password" \
      -p 1433:1433 --name sqlserver \
      mcr.microsoft.com/mssql/server:2022-latest
   ```

2. **Update connection string** in `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "YemekhaneDb": "Server=localhost,1433;Database=YemekhaneDb;User Id=sa;Password=YourStrong@Password;TrustServerCertificate=True;"
     }
   }
   ```

3. **Run the API**
   ```bash
   cd Backend/API/YemekhaneApp.Api
   dotnet run
   ```

4. **Run the Frontend**
   ```bash
   cd UI/YemekhaneApp.Frontend
   dotnet run
   ```

### Production Deployment

For production with pre-built Docker images:

```bash
docker-compose -f docker-compose.server.yml up -d
```

## 📦 Docker Architecture

### Multi-Stage Builds

Both the API and Frontend use optimized multi-stage Docker builds:

- **Base Stage**: Runtime-only ASP.NET Core image
- **Build Stage**: Full SDK for compilation
- **Publish Stage**: Optimized published output
- **Final Stage**: Minimal production image

### Volumes

- `volume_api`: Persistent SQL Server data
- `dataprotection-keys`: Shared data protection keys for distributed deployments

### Networking

All services communicate through a bridge network (`app-network`) for isolation and security.

## 🎨 Key Features

### For Users
- ✅ Secure JWT-based authentication
- ✅ Real-time interactive UI with Blazor
- ✅ Employee management
- ✅ Meal record tracking
- ✅ Debt management system

### For Developers
- ✅ Clean Onion Architecture
- ✅ CQRS pattern with MediatR
- ✅ Repository and Unit of Work patterns
- ✅ Fully containerized with Docker
- ✅ .NET Aspire integration for cloud-native development
- ✅ Comprehensive API documentation with Swagger
- ✅ Entity Framework migrations
- ✅ Background job service for monthly debt calculations

## 🧪 Development Highlights

### CQRS Implementation

The application separates read (Queries) and write (Commands) operations:

```
YemekhaneApp.Application/CQRS/
├── Commands/
│   ├── CreateEmployeeCommand.cs
│   └── UpdateEmployeeCommand.cs
└── Queries/
    ├── GetEmployeeQuery.cs
    └── GetEmployeesQuery.cs
```

### .NET Aspire Integration

The `AspireApp.AppHost` orchestrates the entire application:

```csharp
var db = builder.AddSqlServer("db")
                .AddDatabase("YemekhaneDb");

var api = builder.AddProject<Projects.YemekhaneApp_Api>("api")
                 .WithReference(db);

builder.AddProject<Projects.YemekhaneApp_Frontend>("frontend")
       .WithReference(api);
```

### Dependency Injection

Each layer registers its dependencies independently:

- **Application Layer**: `ApplicationLayerConfig.AddApplicationRegistration()`
- **Persistence Layer**: `PersistenceConfig.AddPersistenceServices()`

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines

- Follow Onion Architecture principles
- Write unit tests for business logic
- Use CQRS pattern for new features
- Document API endpoints with XML comments
- Ensure Docker builds succeed before submitting PR

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🎓 Learning Objectives

This project demonstrates:

- **Clean Architecture**: Onion Architecture with proper dependency inversion
- **CQRS Pattern**: Separation of read and write operations
- **Cloud-Native Development**: .NET Aspire for modern distributed applications
- **Containerization**: Docker best practices with multi-stage builds
- **Security**: JWT authentication and authorization
- **Persistence Patterns**: Repository, Unit of Work, and EF Core
- **Modern UI**: Blazor Server with interactive components

## 🔗 Related Resources

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Onion Architecture](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)

---

**Built with ❤️ using modern .NET technologies**
