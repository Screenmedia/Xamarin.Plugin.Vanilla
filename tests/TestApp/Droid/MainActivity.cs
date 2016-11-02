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
		int count = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.main);

			var button = FindViewById<Button>(Resource.Id.myButton);
			button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
		}
	}
}
