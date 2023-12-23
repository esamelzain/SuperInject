using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperInject.Extensions
{
    public static class SuperExtensions
    {
        public static void AddSuperInject(this IServiceCollection services)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && (Attribute.IsDefined(p, typeof(ServiceAttribute)) || Attribute.IsDefined(p, typeof(RepositoryAttribute))));

            foreach (var type in types)
            {
                try
                {
                    RegisterServiceOrRepository(services, type, new HashSet<Type>());
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    Console.WriteLine($"Error registering {type}: {ex.Message}");
                }
            }
        }

        private static void RegisterServiceOrRepository(IServiceCollection services, Type implementationType, HashSet<Type> processedTypes)
        {
            if (processedTypes.Contains(implementationType))
            {
                // Avoid infinite recursion if there's a circular dependency
                return;
            }

            processedTypes.Add(implementationType);

            var serviceAttribute = (ServiceAttribute?)Attribute.GetCustomAttribute(implementationType, typeof(ServiceAttribute));
            var repositoryAttribute = (RepositoryAttribute?)Attribute.GetCustomAttribute(implementationType, typeof(RepositoryAttribute));

            // Register repository first
            if (repositoryAttribute != null)
            {

                // Check and register dependencies for repositories
                var constructor = implementationType.GetConstructors().FirstOrDefault();

                if (constructor != null)
                {
                    foreach (var parameter in constructor.GetParameters())
                    {
                        if (parameter.ParameterType.IsInterface)
                        {
                            var constructorImplementations = AppDomain.CurrentDomain.GetAssemblies()
                                                    .SelectMany(s => s.GetTypes())
                                                    .Where(t => parameter.ParameterType.IsAssignableFrom(t) && t.IsClass);
                            foreach (var constructorImplementation in constructorImplementations)
                            {
                                RegisterServiceOrRepository(services, constructorImplementation, processedTypes);
                            }
                        }
                        else
                            RegisterServiceOrRepository(services, parameter.ParameterType, processedTypes);
                    }
                }
                RegisterType(services, implementationType, repositoryAttribute.ServiceLifetime);

            }

            // Then register service
            if (serviceAttribute != null)
            {
                // Check and register dependencies for services
                var constructor = implementationType.GetConstructors().FirstOrDefault();

                if (constructor != null)
                {
                    foreach (var parameter in constructor.GetParameters())
                    {
                        if (parameter.ParameterType.IsInterface)
                        {
                            var constructorImplementations = AppDomain.CurrentDomain.GetAssemblies()
                                                    .SelectMany(s => s.GetTypes())
                                                    .Where(t => parameter.ParameterType.IsAssignableFrom(t) && t.IsClass);
                            foreach (var constructorImplementation in constructorImplementations)
                            {
                                RegisterServiceOrRepository(services, constructorImplementation, processedTypes);
                            }
                        }
                        else
                            RegisterServiceOrRepository(services, parameter.ParameterType, processedTypes);
                    }
                }
                RegisterType(services, implementationType, serviceAttribute.ServiceLifetime);
            }
        }

        private static void RegisterType(IServiceCollection services, Type implementationType, ServiceLifetime serviceLifetime)
        {
            var implementedInterfaces = implementationType.GetInterfaces();

            if (implementedInterfaces.Length > 0)
            {
                foreach (var implementedInterface in implementedInterfaces)
                {
                    services.Add(new ServiceDescriptor(implementedInterface, implementationType, serviceLifetime));
                }
            }
            else
            {
                services.Add(new ServiceDescriptor(implementationType, implementationType, serviceLifetime));
            }
        }
    }
}