using Screenmedia.Plugin.Vanilla.Abstractions;
using Screenmedia.Plugin.Vanilla;

namespace VanillaTest
{
	public class ViewModel
	{
		private IIceCreamMachine _iceCreamMachine;

		public ViewModel()
		{
			_iceCreamMachine = new IceCreamMachine();
			IceCreamFlavour = _iceCreamMachine.Dispense();
		}

		public string IceCreamFlavour { get; set; } = "Empty Cone";
	}
}
