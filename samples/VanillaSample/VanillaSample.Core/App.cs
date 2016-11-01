using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using Screenmedia.Plugin.Vanilla;
using Screenmedia.Plugin.Vanilla.Abstractions;

namespace VanillaSample.Core
{
	public class App : MvvmCross.Core.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
			CreatableTypes()
				.EndingWith("Service")
				.AsInterfaces()
				.RegisterAsLazySingleton();

			Mvx.LazyConstructAndRegisterSingleton<IIceCreamMachine>(() => new IceCreamMachine());

			RegisterAppStart<ViewModels.FirstViewModel>();
		}
	}
}
