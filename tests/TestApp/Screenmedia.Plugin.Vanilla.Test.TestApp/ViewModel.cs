// -----------------------------------------------------------------------
//  <copyright file="ViewModel.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2017. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Screenmedia.Plugin.Vanilla.Test.TestApp
{
    using Screenmedia.Plugin.Vanilla.Abstractions;

    public class ViewModel
    {
        private IIceCreamMachine _iceCreamMachine;

        public ViewModel()
        {
            _iceCreamMachine = CrossIceCreamMachine.Current;
            IceCreamFlavour = _iceCreamMachine.Dispense();
        }

        public string IceCreamFlavour { get; set; } = "Empty Cone";
    }
}
