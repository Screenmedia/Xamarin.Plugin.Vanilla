using System;
using UIKit;

namespace VanillaTests.iOS
{
	public partial class ViewController : UIViewController
	{
		public ViewController(IntPtr handle) : base(handle)
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
