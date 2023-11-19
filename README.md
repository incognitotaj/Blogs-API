**EmpowerID .NET Core/Architect Interview Task**

There is a .NET Core API which implemented **Blogging App** with **Entity Framework Core** as ORM, **Microsoft SQL Server** and **Redis Cache** with communicating standard HTTP / HTTPS.

## Whats Including In This Repository
We have implemented below **features over the blogs-api repository**.


#### Blogs API which includes; 
* Implementing **DDD, CQRS, and Clean Architecture** with using Best Practices
* Developing **CQRS with using MediatR, and AutoMapper packages**
* ASP.NET Core Web API application 
* REST API principles, CRUD operations
* **SQL Server Database** connection 
* Using **Entity Framework Core ORM** and auto migrate to SqlServer when application startup
* Repository Pattern Implementation
* Swagger Open API implementation	
* API Containerization


# Run The Project
You will need the following tools:

* [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
* [.Net Core 7](https://dotnet.microsoft.com/download/dotnet-core/7)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Installing
Follow these steps to get your development environment set up: (Before Run Start the Docker Desktop)
1. Clone the repository
2. Once Docker for Windows is installed, go to the **Settings > Advanced option**, from the Docker icon in the system tray, to configure the minimum amount of memory and CPU like so:
* **Memory: 4 GB**
* CPU: 2
3. At the root directory which include **docker-compose.yml** files, run below command:
```csharp
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

>Note: If you get connection timeout error Docker for Mac please [Turn Off Docker's "Experimental Features".](https://github.com/aspnetrun/run-aspnetcore-microservices/issues/33)

4. Wait for docker compose api. That’s it!

5. You can **launch microservices** as below urls:

* **Blogs API -> http://host.docker.internal:<PORT>/index.html**


## Authors

* **Salman Taj** - *Initial work* - [incognitotaj](https://github.com/incognitotaj)
