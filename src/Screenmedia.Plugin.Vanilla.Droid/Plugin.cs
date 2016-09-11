using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Screenmedia.Plugin.Vanilla.Droid
{

	public class Plugin : IMvxPlugin
	{
		public void Load()
		{
			Mvx.RegisterSingleton<IIceCreamMachine>(new IceCreamMachine());
		}
	}
}
