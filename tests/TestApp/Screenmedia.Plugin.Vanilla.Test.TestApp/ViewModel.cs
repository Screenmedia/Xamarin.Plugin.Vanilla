using Screenmedia.Plugin.Vanilla.Abstractions;

namespace Screenmedia.Plugin.Vanilla.Test.TestApp
{
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
