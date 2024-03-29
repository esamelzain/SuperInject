﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SuperInject.Extensions
{
    public static class SuperExtensions
    {
        #region Method

        /// <summary>
        /// Register the superinject service.
        /// </summary>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="superInjectOptions">SuperInjectOptions as delegate action.</param>
        /// <exception cref="ArgumentNullException">When failr it thrown ArgumentNullException.</exception>
        public static void AddSuperInject(this IServiceCollection services, Action<SuperInjectOptions>? superInjectOptions = null)
        {
            Assembly[] Assemblies;
            
            if(superInjectOptions != null)
            {
                var opts = new SuperInjectOptions();
                services.AddSingleton(opts);
                superInjectOptions?.Invoke(opts);
                Assemblies = opts.Assemblies;
            }
            else
                Assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var repositoryTypes = Assemblies
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && Attribute.IsDefined(p, typeof(RepositoryAttribute)));

            foreach (var type in repositoryTypes)
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

            var serviceTypes = Assemblies
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && Attribute.IsDefined(p, typeof(ServiceAttribute)));

            foreach (var type in serviceTypes)
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
        #endregion

        #region Utilities

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
                    if (implementedInterface.IsGenericType)
                    {
                        // Handle generic interface
                        var openGenericType = implementedInterface.GetGenericTypeDefinition();
                        services.Add(new ServiceDescriptor(openGenericType, implementationType, serviceLifetime));
                    }
                    else
                    {
                        services.Add(new ServiceDescriptor(implementedInterface, implementationType, serviceLifetime));
                    }
                }
            }
            else
            {
                //Generic & Non-generic type without interface
                services.Add(new ServiceDescriptor(implementationType, implementationType, serviceLifetime));
            }
        }
        #endregion
    }
}