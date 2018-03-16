// -----------------------------------------------------------------------
//  <copyright file="MainActivity.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2017. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Screenmedia.Plugin.Vanilla.Test.UnitTest.Android
{
    using System.Reflection;
    using global::Android.App;
    using global::Android.OS;
    using Xamarin.Android.NUnitLite;

    [Activity(Label = "Screenmedia.Plugin.Vanilla.Test.UnitTest.Android", MainLauncher = true)]
    public class MainActivity : TestSuiteActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // tests can be inside the main assembly
            AddTest(Assembly.GetExecutingAssembly());
            //// or in any reference assemblies
            //// AddTest (typeof (Your.Library.TestClass).Assembly);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);
        }
    }
}
