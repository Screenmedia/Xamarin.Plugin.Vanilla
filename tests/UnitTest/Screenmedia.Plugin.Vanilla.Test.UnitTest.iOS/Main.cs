// -----------------------------------------------------------------------
//  <copyright file="Main.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2018. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Reviewed")]
namespace Screenmedia.Plugin.Vanilla.Test.UnitTest
{
    using UIKit;

    public class Application
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "UnitTestAppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
