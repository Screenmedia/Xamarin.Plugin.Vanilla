// -----------------------------------------------------------------------
//  <copyright file="CrossIceCreamMachine.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2017. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Screenmedia.Plugin.Vanilla
{
    using System;
    using Screenmedia.Plugin.Vanilla.Abstractions;

    /// <summary>
    /// Cross platform IceCreamMachine implemenations
    /// </summary>
    public static class CrossIceCreamMachine
    {
        private static Lazy<IIceCreamMachine> implementation = new Lazy<IIceCreamMachine>(CreateIceCreamMachine, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IIceCreamMachine Current
        {
            get
            {
                var ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }

                return ret;
            }
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }

        private static IIceCreamMachine CreateIceCreamMachine()
        {
#if NETSTANDARD1_0
            return null;
#else
            return new IceCreamMachineImplementation();
#endif
        }
    }
}
