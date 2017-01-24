using Screenmedia.Plugin.Vanilla.Abstractions;

namespace Screenmedia.Plugin.Vanilla
{
	public class IceCreamMachineImplementation : IIceCreamMachine
	{
		public string Dispense()
		{
			return "Chocolate";
		}
	}
}
