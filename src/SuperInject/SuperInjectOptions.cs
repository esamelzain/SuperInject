
using System.Reflection;

namespace SuperInject
{

    /// <summary>
    /// A class define variouse data to configure the Superinject core services.
    /// </summary>
    public class SuperInjectOptions
    {
        /// <summary>
        /// Get or set the assemblies list to scan and configure the DI services inside them.
        /// </summary>
        public Assembly[] Assemblies { get; set; } = default!;
    }
}