// -----------------------------------------------------------------------
//  <copyright file="AppDelegate.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2018. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Screenmedia.Plugin.Vanilla.Test.UnitTest
{
    using System.Reflection;
    using Foundation;
    using UIKit;
    using Xunit.Runner;
    using Xunit.Sdk;

    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : RunnerAppDelegate
    {
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);
            AddTestAssembly(Assembly.GetExecutingAssembly());
            return base.FinishedLaunching(app, options);
        }
    }
}
