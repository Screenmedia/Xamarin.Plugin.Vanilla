using System;
using UIKit;
using VanillaSample.Core.ViewModels;
using Screenmedia.Plugin.Vanilla;

namespace VanillaSample.iOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			var vm = new FirstViewModel(); 
			Label.Text = vm.IceCream;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		}
	}
}
