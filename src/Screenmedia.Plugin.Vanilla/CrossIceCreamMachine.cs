﻿﻿using System;
using Screenmedia.Plugin.Vanilla.Abstractions;

namespace Screenmedia.Plugin.Vanilla
{
    /// <summary>
    /// Cross platform IceCreamMachine implemenations
    /// </summary>
    public static class CrossIceCreamMachine
    {
        static Lazy<IIceCreamMachine> Implementation = new Lazy<IIceCreamMachine>(CreateIceCreamMachine, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IIceCreamMachine Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IIceCreamMachine CreateIceCreamMachine()
        {
#if NETSTANDARD1_0
            return null;
#else
            return new IceCreamMachineImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
