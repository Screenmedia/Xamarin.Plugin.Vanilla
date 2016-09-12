using MvvmCross.Core.ViewModels;
using Screenmedia.Plugin.Vanilla;
using MvvmCross.Platform;

namespace VanillaSample.Core.ViewModels
{
	public class FirstViewModel
		: MvxViewModel
	{
		readonly IIceCreamMachine _iceCreamMachine;

		public FirstViewModel()
		{
			_iceCreamMachine = Mvx.Resolve<IIceCreamMachine>();
			Hello = _iceCreamMachine.Dispense();
		}

		private string _hello = "Hello MvvmCross";
		public string Hello
		{
			get { return _hello; }
			set { SetProperty(ref _hello, value); }
		}
	}
}
