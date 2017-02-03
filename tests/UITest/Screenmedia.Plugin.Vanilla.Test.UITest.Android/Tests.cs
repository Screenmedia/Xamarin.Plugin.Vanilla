using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;

namespace Screenmedia.Plugin.Vanilla.Test.UITest.Android
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            // TODO: If the Android app being tested is included in the solution then open
            // the Unit Tests window, right click Test Apps, select Add App Project
            // and select the app projects that should be tested.
            app = ConfigureApp
                .Android
                // TODO: Update this path to point to your Android app and uncomment the
                // code if the app is not included in the solution.
                .ApkFile ("../../../../TestApp/Screenmedia.Plugin.Vanilla.Test.TestApp.Android/bin/Release/uk.co.screenmedia.plugin.vanilla.test-Signed.apk")
                .StartApp();
        }

        [Test]
        public void DispenseCorrectIceCream()
        {
            app.WaitForElement(c => c.Marked("Chocolate"));
        }
    }
}
