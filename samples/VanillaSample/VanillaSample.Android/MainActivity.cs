using Android.App;
using Android.Widget;
using Android.OS;
using VanillaSample.Core.ViewModels;

namespace VanillaSample.Android
{
	[Activity(
		  MainLauncher = true
		, Icon = "@mipmap/icon"
	)]

	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.main);

			var vm = new ViewModel();
			var button = FindViewById<TextView>(Resource.Id.textView1);
			button.Text = vm.IceCreamFlavour;
		}
	}
}

