# Commander-.NET-Core-3.1-MVC-REST-API

Overview

Welcome to the Commander API, a powerful RESTful web service built on .NET Core 3.1. This API utilizes a range of modern technologies, including MVC, REST architecture, the Repository Pattern, Dependency Injection, Entity Framework, Data Transfer Objects (DTOs), and AutoMapper. It provides six essential API endpoints for managing resources: Create, Read, Update, and Delete. Additionally, this project is equipped with a robust unit testing framework that covers the Controller HTTP methods.
Technologies Used

    .NET Core 3.1: The foundation for building the API.
    MVC (Model-View-Controller): A design pattern used for structuring the application.
    REST: A set of architectural constraints for designing networked applications.
    Repository Pattern: A design pattern that separates the data access logic from the rest of the application.
    Dependency Injection: Utilized to manage and inject dependencies.
    Entity Framework: An ORM framework for database interactions.
    Data Transfer Objects (DTOs): Used to exchange data between the API and clients.
    AutoMapper: A library for object-to-object mapping.
    Postman: A tool for testing and documenting APIs.

Understanding REST

The API follows the principles of Representational State Transfer (REST):

    Separation of Client and Server: The client and server are separate entities.
    Stateless Server Requests: Each request to the server is independent and does not rely on previous requests.
    Cacheable Requests: Responses can be cached to improve performance.
    Uniform Interface: A consistent and uniform way to interact with resources.

API Endpoints

The API provides the following endpoints:

    GET: Retrieve a resource.
    POST: Add a new resource.
    PUT: Update an existing resource.
    Patch: Update an existing resource with a set of changes.
    DELETE: Remove an existing resource.

Unit Testing

To ensure the reliability and robustness of the API, we have incorporated a comprehensive unit testing framework that covers the Controller HTTP methods. These unit tests play a vital role in maintaining the quality and stability of the API.
What is .NET Core?

    API: Stands for Application Programming Interface, which facilitates communication between different parts of a software system.
    SDK: Software Development Kit, a collection of tools and libraries to aid in software development.
    Cross-platform: .NET Core is platform-agnostic and can run on Windows, Mac, and Linux.
    Open Source: The source code for .NET Core is open for the community to view and contribute to.

Migration to .NET Core

The repository also provides information about migrating to .NET Core from previous .NET implementations, including the .NET Framework and .NET Standard.
Already Implemented

This project already incorporates the following elements:

    Dependency Injection: Utilizes the built-in DI container for managing dependencies.
    Swagger: API documentation with Swagger has been implemented.
    Global Exception Handling: Global exception handling is in place.
    AutoMapper: AutoMapper is used for object mapping.
    Unit Testing: Unit tests for the Controller HTTP methods are included.
    Basic Authentication: Basic authentication is applied for enhanced security.
