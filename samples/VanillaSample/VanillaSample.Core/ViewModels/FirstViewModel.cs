using MvvmCross.Core.ViewModels;
using Screenmedia.Plugin.Vanilla.Abstractions;

namespace VanillaSample.Core.ViewModels
{
	public class FirstViewModel
		: MvxViewModel
	{
		readonly IIceCreamMachine _iceCreamMachine;

		public FirstViewModel(IIceCreamMachine iceCreamMachine)
		{
			_iceCreamMachine = iceCreamMachine;
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
