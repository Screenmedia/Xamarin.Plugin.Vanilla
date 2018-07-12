// -----------------------------------------------------------------------
//  <copyright file="ViewModel.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2018. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace VanillaSample.Core.ViewModels
{
    using Screenmedia.Plugin.Vanilla;
    using Screenmedia.Plugin.Vanilla.Abstractions;

    public class ViewModel
    {
        private readonly IIceCreamMachine _iceCreamMachine;

        public ViewModel()
        {
            _iceCreamMachine = CrossIceCreamMachine.Current;
            IceCreamFlavour = _iceCreamMachine.Dispense();
        }

        public string IceCreamFlavour { get; set; } = "Empty Cone";
    }
}
