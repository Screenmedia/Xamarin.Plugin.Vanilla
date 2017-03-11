#addin Cake.FileHelpers
using System.Text.RegularExpressions;
var TARGET = Argument ("target", Argument ("t", "Default"));

var isJenkinsBuild = Jenkins.IsRunningOnJenkins;
var version = "0.0.0"; // this will be overridden by the changelog enteries
var pluginName = "Vanilla";
var packageName =  "Screenmedia.Plugin." + pluginName;

var src = new Dictionary<string, string> {
 	{ "./src/"+ packageName + ".sln", "Any" },
};

var BuildAction = new Action<Dictionary<string, string>> (solutions =>
{
	foreach (var sln in solutions) 
    {
		// If the platform is Any build regardless
		//  If the platform is Win and we are running on windows build
		//  If the platform is Mac and we are running on Mac, build
		if ((sln.Value == "Any")
				|| (sln.Value == "Win" && IsRunningOnWindows ())
				|| (sln.Value == "Mac" && IsRunningOnUnix ())) 
        {
			// Bit of a hack to use nuget3 to restore packages for project.json
			if (IsRunningOnWindows ()) 
            {
				Information ("RunningOn: {0}", "Windows");

				NuGetRestore (sln.Key, new NuGetRestoreSettings
                {
					ToolPath = "./tools/nuget3.exe"
				});

				// Windows Phone / Universal projects require not using the amd64 msbuild
				MSBuild (sln.Key, c => 
                { 
					c.Configuration = "Release";
					c.MSBuildPlatform = Cake.Common.Tools.MSBuild.MSBuildPlatform.x86;
				});
			} 
            else 
            {
                // Mac is easy ;)
				NuGetRestore (sln.Key);

				DotNetBuild (sln.Key, c => c.Configuration = "Release");
			}
		}
	}
});

Task("srcBuild").Does(()=>
{
    BuildAction(src);
});

Task ("NuGet")
	.IsDependentOn ("srcBuild")
	.Does (() =>
{
    if(!DirectoryExists("./Build/nuget/")) CreateDirectory("./Build/nuget");

	var latestChange = FileReadLines("./changelog")[0];

	// Taking version number from changelog
    Match match = Regex.Match(latestChange, @"\[([^)]*)\]");
	if (match.Success)
    {
		version = match.Groups[1].Value;
        Information($"Publishing Version: {version}");
    }

    var nugetPackSettings = new NuGetPackSettings 
    { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./Build/nuget/",
		BasePath = "./",
		ReleaseNotes = FileReadLines("./changelog") // takes the first line the change log
	};

	var nugetPushSettings = new NuGetPushSettings {
		Source = EnvironmentVariable("MYGET_SERVER"),
		ApiKey = EnvironmentVariable("MYGET_APIKEY")
	};

	if(IsRunningOnWindows ()) 
    {
		nugetPackSettings.ToolPath = "./tools/nuget3.exe";
		nugetPushSettings.ToolPath = "./tools/nuget3.exe";
    }

	NuGetPack ("./nuget/" + packageName + ".nuspec", nugetPackSettings);	

	// publish
	// Get the path to the package.
	var package = "./Build/nuget/" + packageName + "." + version + ".nupkg";
					
	if(EnvironmentVariable("MYGET_PUSH") != null)
	{
		// Push the package.
		NuGetPush(package, nugetPushSettings);
	}
});

//Build the src and deploy to nuget
Task ("Default").IsDependentOn("NuGet");


Task ("Clean").Does (() => 
{
	CleanDirectories ("./Build/");
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});


RunTarget (TARGET);
