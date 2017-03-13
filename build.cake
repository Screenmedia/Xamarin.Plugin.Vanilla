#addin Cake.FileHelpers
using System.Text.RegularExpressions;
var TARGET = Argument ("target", Argument ("t", "Default"));

var isJenkinsBuild = Jenkins.IsRunningOnJenkins;
var version = "0.0.0"; // this will be overridden by the changelog entries
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
    if(!DirectoryExists("./Build/nuget/")) 
	{
		CreateDirectory("./Build/nuget");
	}

	var changeLogPath = "./changelog";
	if(!FileExists(changeLogPath))
	{
		throw new Exception(@"No Change log file found, please add the file 'changelog' to the root directory");
	}

	var changes = FileReadLines(changeLogPath);
	string latestChange = String.Empty;
	if (changes.Length > 0)
	{
		latestChange = changes[0]; // takes the first line the change log
        Information($"Nuegt Package Release notes: {latestChange}");
	}
	else
	{
		throw new Exception("No entries in change log file found, please add an entry e.g. [1.0.0] added a wicked left hand menu");
	}

	//Taking version number from changelog
	var semverRegex = @"\[(?<whole>(?<major>\d+)(\.(?<minor>\d+))?(\.(?<patch>\d+))?(\-(?<pre>[0-9A-Za-z\-\.]+))?(\+(?<build>[0-9A-Za-z\-\.]+))?)\]\s+(?<notes>.*)";
	Match match = Regex.Match(latestChange, semverRegex);
	if (match.Success)
    {
		version = match.Groups["whole"].Value;
        Information($"Publishing Version: {version}");
    }
	else
	{
		throw new Exception("No version number detected in changelog, please add a semantic version in the change log like so [1.0.0] added a sweet temperature picker");
	}


    var nugetPackSettings = new NuGetPackSettings 
    { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./Build/nuget/",
		BasePath = "./",
		ReleaseNotes = new string[] {latestChange}
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
