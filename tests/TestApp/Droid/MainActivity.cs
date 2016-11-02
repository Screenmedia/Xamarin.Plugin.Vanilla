using Android.App;
using Android.Widget;
using Android.OS;

namespace VanillaTests.Droid
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
			var textView = FindViewById<TextView>(Resource.Id.textView1);
			textView.Text = vm.IceCreamFlavour;
		}
	}
}
