using Microsoft.Extensions.DependencyInjection;
using SuperInject.Interfaces;
using System;

namespace SuperInject
{
    public class RepositoryAttribute : Attribute, ISuperInject
    {
        public ServiceLifetime ServiceLifetime { get; }

        public RepositoryAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ServiceLifetime = serviceLifetime;
        }
    }
}
