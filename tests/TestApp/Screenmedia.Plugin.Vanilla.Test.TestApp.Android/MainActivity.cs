// -----------------------------------------------------------------------
//  <copyright file="MainActivity.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2017. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Screenmedia.Plugin.Vanilla.Test.TestApp.Android
{
    using global::Android.App;
    using global::Android.Widget;
    using global::Android.OS;

    [Activity(
        MainLauncher = true,
        Icon = "@mipmap/icon")]
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
