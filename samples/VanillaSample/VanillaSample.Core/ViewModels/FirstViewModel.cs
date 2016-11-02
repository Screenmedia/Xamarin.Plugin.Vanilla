using Screenmedia.Plugin.Vanilla;
using Screenmedia.Plugin.Vanilla.Abstractions;

namespace VanillaSample.Core.ViewModels
{
	public class FirstViewModel
	{
		readonly IIceCreamMachine _iceCreamMachine;

		public FirstViewModel()
		{
			_iceCreamMachine = new IceCreamMachine();
			IceCream = _iceCreamMachine.Dispense();
		}

		public string IceCream { get; set; } = "Empty Cone";
	}
}
