# Archse
```mermaid
C4Context
    title Microservice Architecture with Microfrontend

    Person(user, "User", "Interacts with the application via the microfrontend")
    
    System_Boundary(Frontend_Boundary, "Microfrontend") {
        Container(microfrontend, "Microfrontend", "React", "Provides the user interface and communicates with the API Gateway")
    }
    
    System_Boundary(APIGateway_Boundary, "API Gateway") {
        Container(apigateway, "API Gateway", "OCELOT", "Routes requests to the BFF and handles cross-cutting concerns")
    }
    
    System_Boundary(BFF_Boundary, "BFF") {
        Container(bff, "BFF", "Node.js", "Acts as an intermediary between the API Gateway and backend microservices")
    }

    System_Boundary(Backend_Services, "Microservices") {
        Container(microservice1, "Microservice A",".NET" "Handles specific business logic")
        Container(microservice2, "Microservice B",".NET", "Handles specific business logic")
        Container(microservice3, "Microservice C", ".NET", "Handles specific business logic")
    }
    
    System_Ext(bdd1, "Database A", "SQL Server", "Stores persistent data for Microservice A")
    System_Ext(bdd2, "Database B", "SQL Server", "Stores persistent data for Microservice B")
    System_Ext(bdd3, "Database C", "SQL Server", "Stores persistent data for Microservice C")
    System_Ext(redis, "Redis Cache", "Redis", "Provides caching for fast data access")
    System_Ext(servicebus, "Azure Service Bus", "Azure Service Bus", "Handles asynchronous message communication between microservices")

    Rel(user, microfrontend, "Uses")
    Rel(microfrontend, apigateway, "Sends requests to")
    Rel(apigateway, bff, "Routes requests to")
    Rel(bff, microservice1, "Forwards requests to")
    Rel(bff, microservice2, "Forwards requests to")
    Rel(bff, microservice3, "Forwards requests to")
    Rel(microservice1, bdd1, "Reads from and writes to")
    Rel(microservice2, bdd2, "Reads from and writes to")
    Rel(microservice3, bdd3, "Reads from and writes to")
    Rel(microservice1, redis, "Uses for caching")
    Rel(microservice2, redis, "Uses for caching")
    Rel(microservice3, redis, "Uses for caching")
    Rel(microservice1, servicebus, "Sends and receives messages")
    Rel(microservice2, servicebus, "Sends and receives messages")
    Rel(microservice3, servicebus, "Sends and receives messages")
