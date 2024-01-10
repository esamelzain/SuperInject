using Microsoft.Extensions.DependencyInjection;
using SuperInject.Interfaces;
using System;

namespace SuperInject
{
    public class ServiceAttribute : Attribute, ISuperInject
    {
        public ServiceLifetime ServiceLifetime { get; }

        public ServiceAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ServiceLifetime = serviceLifetime;
        }
    }
}
