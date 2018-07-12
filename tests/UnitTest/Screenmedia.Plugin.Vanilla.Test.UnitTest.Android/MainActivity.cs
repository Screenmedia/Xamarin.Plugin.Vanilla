// -----------------------------------------------------------------------
//  <copyright file="MainActivity.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2017. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Screenmedia.Plugin.Vanilla.Test.UnitTest
{
    using System.Reflection;
    using global::Android.App;
    using global::Android.OS;
    using Xunit.Runners.UI;

    [Activity(MainLauncher = true)]
    public class MainActivity : RunnerActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            AddTestAssembly(Assembly.GetExecutingAssembly());

            // you cannot add more assemblies once calling base
            base.OnCreate(bundle);
        }
    }
}
