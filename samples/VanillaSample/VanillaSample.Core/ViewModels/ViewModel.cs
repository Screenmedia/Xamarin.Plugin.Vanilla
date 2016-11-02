using Screenmedia.Plugin.Vanilla;
using Screenmedia.Plugin.Vanilla.Abstractions;

namespace VanillaSample.Core.ViewModels
{
	public class ViewModel
	{
		readonly IIceCreamMachine _iceCreamMachine;

		public ViewModel()
		{
			_iceCreamMachine = new IceCreamMachine();
			IceCreamFlavour = _iceCreamMachine.Dispense();
		}

		public string IceCreamFlavour { get; set; } = "Empty Cone";
	}
}
