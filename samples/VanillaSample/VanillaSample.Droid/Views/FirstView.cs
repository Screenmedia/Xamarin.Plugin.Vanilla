using Android.App;
using Android.OS;

namespace VanillaSample.Droid.Views
{
	[Activity]
	public class FirstView : BaseView
	{
		protected override int LayoutResource => Resource.Layout.first_view;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SupportActionBar.SetDisplayHomeAsUpEnabled(false);
		}
	}
}
