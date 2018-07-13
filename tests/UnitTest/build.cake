#addin nuget:?package=Cake.AppleSimulator
#addin nuget:?package=Cake.Android.Adb&version=2.0.6
#addin nuget:?package=Cake.Android.AvdManager&version=1.0.3
#addin nuget:?package=Cake.FileHelpers

var TARGET = Argument("target", "Default");

var ANDROID_PROJ = "./Screenmedia.Plugin.Vanilla.Test.UnitTest.Android/Screenmedia.Plugin.Vanilla.Test.UnitTest.Android.csproj";
var ANDROID_TEST_RESULTS_PATH = "./xunit-android.xml";
var ANDROID_AVD = "CABOODLE";
var ANDROID_PKG_NAME = "uk.co.screenmedia.plugin.vanilla.test.unittest";
var ANDROID_EMU_TARGET = EnvironmentVariable("ANDROID_EMU_TARGET") ?? "system-images;android-26;google_apis;x86_64";
var ANDROID_EMU_DEVICE = EnvironmentVariable("ANDROID_EMU_DEVICE") ?? "Nexus 5X";

var TCP_LISTEN_TIMEOUT = 60;
var TCP_LISTEN_PORT = 10578;
var TCP_LISTEN_HOST = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
        .AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();

var ANDROID_HOME = EnvironmentVariable("ANDROID_HOME");

Func<int, FilePath, Task> DownloadTcpTextAsync = (int port, FilePath filename) =>
    System.Threading.Tasks.Task.Run (() => {
        var tcpListener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, port);
        tcpListener.Start();
        var listening = true;

        System.Threading.Tasks.Task.Run(() => {
            // Sleep until timeout elapses or tcp listener stopped after a successful connection
            var elapsed = 0;
            while (elapsed <= TCP_LISTEN_TIMEOUT && listening) {
                System.Threading.Thread.Sleep(1000);
                elapsed++;
            }

            // If still listening, timeout elapsed, stop the listener
            if (listening) {
                tcpListener.Stop();
                listening = false;
            }
        });

        try {
            var tcpClient = tcpListener.AcceptTcpClient();
            var fileName = MakeAbsolute (filename).FullPath;

            using (var file = System.IO.File.Open(fileName, System.IO.FileMode.Create))
            using (var stream = tcpClient.GetStream())
                stream.CopyTo(file);

            tcpClient.Close();
            tcpListener.Stop();
            listening = false; 
        } catch {
            throw new Exception("Test results listener failed or timed out.");
        }
    });

Action<FilePath, string> AddPlatformToTestResults = (FilePath testResultsFile, string platformName) => {
    if (FileExists(testResultsFile)) {
        var txt = FileReadText(testResultsFile);
        txt = txt.Replace("<test-case name=\"DeviceTests.", $"<test-case name=\"DeviceTests.{platformName}.");
        txt = txt.Replace("<test name=\"DeviceTests.", $"<test name=\"DeviceTests.{platformName}.");
        txt = txt.Replace("name=\"Test collection for DeviceTests.", $"name=\"Test collection for DeviceTests.{platformName}.");        
        FileWriteText(testResultsFile, txt);
    }
};


Task ("build-android")
    .Does (() =>
{
    // Nuget restore
    MSBuild (ANDROID_PROJ, c => {
        c.Configuration = "Debug";
        c.Targets.Clear();
        c.Targets.Add("Restore");
    });

    // Build the app in debug mode
    // needs to be debug so unit tests get discovered
    MSBuild (ANDROID_PROJ, c => {
        c.Configuration = "Debug";
        c.Targets.Clear();
        c.Targets.Add("Rebuild");
    });
});

Task ("test-android-emu")
    .IsDependentOn ("build-android")
    .Does (() =>
{
    if (EnvironmentVariable("ANDROID_SKIP_AVD_CREATE") == null) {
        var avdSettings = new AndroidAvdManagerToolSettings  { SdkRoot = ANDROID_HOME };

        // Create the AVD if necessary
        Information ("Creating AVD if necessary: {0}...", ANDROID_AVD);
        if (!AndroidAvdListAvds (avdSettings).Any (a => a.Name == ANDROID_AVD))
            AndroidAvdCreate (ANDROID_AVD, ANDROID_EMU_TARGET, ANDROID_EMU_DEVICE, force: true, settings: avdSettings);
    }

    // We need to find `emulator` and the best way is to try within a specified ANDROID_HOME
    var emulatorExt = IsRunningOnWindows() ? ".bat" : "";
    string emulatorPath = "emulator" + emulatorExt;

    if (ANDROID_HOME != null) {
        var andHome = new DirectoryPath(ANDROID_HOME);
        if (DirectoryExists(andHome)) {
            emulatorPath = MakeAbsolute(andHome.Combine("tools").CombineWithFilePath("emulator" + emulatorExt)).FullPath;
            if (!FileExists(emulatorPath))
                emulatorPath = MakeAbsolute(andHome.Combine("emulator").CombineWithFilePath("emulator" + emulatorExt)).FullPath;
            if (!FileExists(emulatorPath))
                emulatorPath = "emulator" + emulatorExt;
        }
    }

    // Start up the emulator by name
    Information ("Starting Emulator: {0}...", ANDROID_AVD);
    var emu = StartAndReturnProcess (emulatorPath, new ProcessSettings { 
        Arguments = $"-avd {ANDROID_AVD}" });

    var adbSettings = new AdbToolSettings { SdkRoot = ANDROID_HOME };

    // Keep checking adb for an emulator with an AVD name matching the one we just started
    var emuSerial = string.Empty;
    for (int i = 0; i < 100; i++) {
        foreach (var device in AdbDevices(adbSettings).Where(d => d.Serial.StartsWith("emulator-"))) {
            if (AdbGetAvdName(device.Serial).Equals(ANDROID_AVD, StringComparison.OrdinalIgnoreCase)) {
                emuSerial = device.Serial;
                break;
            }
        }

        if (!string.IsNullOrEmpty(emuSerial))
            break;
        else
            System.Threading.Thread.Sleep(1000);
    }

    Information ("Matched ADB Serial: {0}", emuSerial);
    adbSettings = new AdbToolSettings { SdkRoot = ANDROID_HOME, Serial = emuSerial };

    // Wait for the emulator to enter a 'booted' state
    AdbWaitForEmulatorToBoot(TimeSpan.FromSeconds(100), adbSettings);
    Information ("Emulator finished booting.");

    // Try uninstalling the existing package (if installed)
    try { 
        AdbUninstall (ANDROID_PKG_NAME, false, adbSettings);
        Information ("Uninstalled old: {0}", ANDROID_PKG_NAME);
    } catch { }

    // Use the Install target to push the app onto emulator
    MSBuild (ANDROID_PROJ, c => {
        c.Configuration = "Debug";
        c.Properties["AdbTarget"] = new List<string> { "-s " + emuSerial };
        c.Targets.Clear();
        c.Targets.Add("Install");
    });

    // Start the TCP Test results listener
    Information("Started TCP Test Results Listener on port: {0}:{1}", TCP_LISTEN_HOST, TCP_LISTEN_PORT);
    var tcpListenerTask = DownloadTcpTextAsync (TCP_LISTEN_PORT, ANDROID_TEST_RESULTS_PATH);

    // Launch the app on the emulator
    AdbShell ($"am start -n {ANDROID_PKG_NAME}/{ANDROID_PKG_NAME}.MainActivity --es HOST_IP {TCP_LISTEN_HOST} --ei HOST_PORT {TCP_LISTEN_PORT}", adbSettings);

    // Wait for the test results to come back
    Information("Waiting for tests...");
    tcpListenerTask.Wait ();

    AddPlatformToTestResults(ANDROID_TEST_RESULTS_PATH, "Android");

    // Close emulator
    emu.Kill();
});


RunTarget(TARGET);
