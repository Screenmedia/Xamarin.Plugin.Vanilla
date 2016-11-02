using Screenmedia.Plugin.Vanilla.Abstractions;
using Screenmedia.Plugin.Vanilla;
namespace VanillaTests
{
	public class ViewModel
	{
		private IIceCreamMachine _iceCreamMachine;

		public ViewModel()
		{
			_iceCreamMachine = new IceCreamMachine();
		}

		public string IceCreamFlavour { get; set; } = "Empty Cone";
	}
}
