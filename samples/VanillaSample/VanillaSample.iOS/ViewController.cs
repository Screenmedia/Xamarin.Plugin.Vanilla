using System;
using UIKit;
using VanillaSample.Core.ViewModels;

namespace VanillaSample.iOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{ 
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			var vm = new ViewModel();
			Label.Text = vm.IceCreamFlavour;
		}
	}
}
