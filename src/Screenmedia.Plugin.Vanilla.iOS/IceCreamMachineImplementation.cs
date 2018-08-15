// -----------------------------------------------------------------------
//  <copyright file="IceCreamMachineImplementation.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2018. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Screenmedia.Plugin.Vanilla
{
    using Screenmedia.Plugin.Vanilla.Abstractions;

    public class IceCreamMachineImplementation : IIceCreamMachine
    {
        public string Dispense()
        {
            return "Strawberry";
        }
    }
}
