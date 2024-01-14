using Microsoft.Extensions.DependencyInjection;
using System;
namespace SuperInject
{

    [AttributeUsage(AttributeTargets.Class)]
    public class WithSuperInjectAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; }

        public WithSuperInjectAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ServiceLifetime = serviceLifetime;
        }
    }
}