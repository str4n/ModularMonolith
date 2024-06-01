# ModularMonolith

"ModularMonolith" provides a well-structured foundation for building an ASP.NET Core application using a modular monolith architecture. It is written in **[.NET 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)**.

The **modular monolith** approach offers the benefits of code organization and maintainability, while preserving the advantages of a single codebase for easier deployment and management.

# Project Structure:

## Bootstrapper:
+ The heart of the application.
+ Coordinates startup tasks, including loading modules.
+ Serves as the entry point for the application.
  
## Shared.Abstractions:
+ Defines core interfaces and abstractions used throughout the application.
+ Fosters loose coupling between modules.
  
## Shared.Infrastructure:
+ Houses concrete implementations of essential infrastructure components.
+ Includes:
  + **Postgres Database**: Connection and interaction logic for PostgreSQL.
  + **In-Memory Message Broker**: Very simple implementation of message broker in memory (It can be easily replaced with some more complex solution like RabbitMQ)
  + **CQRS approach**: Custom implementation for handling commands and queries within the application.
  + **Redis Cache**: Caching implementation using Redis for performance optimization.
  + **Logging**: Logging via Serilog integrated with [Seq](https://datalust.co/seq) for centralized logging and visualization.
+ Promotes reusability and centralized management of infrastructure concerns.

# Development:

+ You can create modules within the project structure to encapsulate specific functionalities or business domains.
  + To load module using **Bootstrapper**, just add module dependecy to the **Bootstrapper**, then add following line to the Program.cs file:
    
    ```

    ModuleLoader.Load<SampleModule>();
  
    ```

  + You can find sample module integration on [**Sample Module**](https://github.com/str4n/ModularMonolith/tree/SampleModule) branch

  
+ Enhance the Shared.Infrastructure project with new infrastructure components as needed for your application's requirements.

# Running the application:

## 1. Prerequisites:

+ [.NET Core SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0).
+ Code editor or IDE (e.g., Visual Studio, Visual Studio Code)
+ [Docker Desktop](https://www.docker.com/)

## 2. Clone the Repository:

```

git clone https://github.com/str4n/ModularMonolith.git

```

## 3. Start the infrastructure (Postgres, Redis, Seq) using [Docker](https://www.docker.com/).

```

cd ModularMonolith
docker-compose up -d

```
