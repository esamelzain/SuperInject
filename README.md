##SuperInject NuGet Library

The SuperInject library provides a convenient way to automatically register services and repositories in the Microsoft.Extensions.DependencyInjection container using custom attributes. It simplifies the dependency registration process, allowing developers to annotate their classes with attributes and then automatically register those classes with the dependency injection container.
## Getting Started
### Installation
To use the SuperInject library in your project, install the NuGet package:

dotnet add package SuperInject 

### Usage
In your project, use the SuperInject library to automatically register services and repositories by marking your classes with the appropriate attributes.
## Attributes
### ServiceAttribute
The **ServiceAttribute** is used to mark classes as services and define their injection type.
#### ***Constructor***
- **ServiceAttribute(ServiceLifetime serviceLifetime)**: Creates a new instance of the **ServiceAttribute** with the specified **ServiceLifetime**.
### RepositoryAttribute
The **RepositoryAttribute** is used to mark classes as repositories and define their injection type.
#### ***Constructor***
- **RepositoryAttribute(ServiceLifetime serviceLifetime)**: Creates a new instance of the **RepositoryAttribute** with the specified **ServiceLifetime**.
## Extension Method
### AddSuperInject
The **AddSuperInject** extension method simplifies the process of automatically registering services and repositories in the dependency injection container.
#### ***Method Signature***

public static void AddSuperInject(this IServiceCollection services); 
#### ***Description***
This method scans the assemblies in the current domain for classes marked with **ServiceAttribute** or **RepositoryAttribute**. It then registers these classes in the dependency injection container based on their specified injection type (**ServiceLifetime**).
#### ***Usage Example***

// ConfigureServices method in Startup.cs

public void ConfigureServices(IServiceCollection services)

{

`    `// Use AddSuperInject method to automatically register services and repositories

`    `services.AddSuperInject();

`    `// Your other service registrations go here...

}

## Considerations
While SuperInject simplifies the dependency injection process, it's essential to be mindful of circular dependencies. Circular dependencies can lead to infinite recursion, and though the library attempts to handle them gracefully, it's advisable to design your application to avoid such scenarios.
## Conclusion
The SuperInject NuGet Library provides an elegant solution for managing dependencies in .NET applications. By leveraging attributes and extension methods, developers can effortlessly register services and repositories, enhancing code readability and maintainability. Install SuperInject in your project today to experience the benefits of streamlined dependency injection.
## License
SuperInject is released under the MIT License, offering flexibility for customization and extension to suit your specific requirements.

