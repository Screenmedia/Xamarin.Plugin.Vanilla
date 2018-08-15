// -----------------------------------------------------------------------
//  <copyright file="MainActivity.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2018. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace VanillaSample.Android
{
    using global::Android.App;
    using global::Android.Widget;
    using global::Android.OS;
    using VanillaSample.Core.ViewModels;

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
            var button = FindViewById<TextView>(Resource.Id.textView1);
            button.Text = vm.IceCreamFlavour;
        }
    }
}
