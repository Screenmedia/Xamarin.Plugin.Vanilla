using Android.App;
using Android.Widget;
using Android.OS;
using VanillaSample.Core.ViewModels;

namespace VanillaSample.Android
{
	[Activity(Label = "VanillaSample.Android", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var vm = new FirstViewModel();

			// Get our button from the layout resource,
			// and attach an event to it
			TextView button = FindViewById<TextView>(Resource.Id.textView1);
			button.Text = vm.IceCream;
		}
	}
}

