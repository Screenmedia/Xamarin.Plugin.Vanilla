#addin Cake.FileHelpers
using System.Text.RegularExpressions;

var TARGET = Argument ("target", Argument ("t", "Default"));

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
			NuGetRestore (sln.Key, new NuGetRestoreSettings
			{
				ToolPath = "./tools/nuget.exe"
			});

			MSBuild (sln.Key, c => 
			{
				c.Configuration = "Release";

				if (IsRunningOnWindows ())
				{
					// Windows Phone / Universal projects require not using the amd64 msbuild
					c.MSBuildPlatform = Cake.Common.Tools.MSBuild.MSBuildPlatform.x86;
				}
				else
				{
					// Mac not yet handling finding msbuild by itself
					c.ToolPath = @"/Library/Frameworks/Mono.framework/Versions/Current/Commands/msbuild";
				}
			});
		}
	}
});

Task("srcBuild").Does(()=>
{
    BuildAction(src);
});

Task ("NuGet")
	.IsDependentOn ("srcBuild")
	.WithCriteria(EnvironmentVariable("MYGET_PUSH") != null)
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

	var changesText = FileReadText(changeLogPath);
	if (String.IsNullOrEmpty(changesText))
	{
		throw new Exception("No entries in change log file found, please add an entry e.g. \n\n[1.0.0] - 2017/03/13 \nAdded \n- A wicked left hand menu");
	}

	//Taking version number from changelog
	var version = "0.0.0";
	var notes = string.Empty;
	var semverRegex = @"\[(?<version>(?<major>\d+)(\.(?<minor>\d+))?(\.(?<patch>\d+))?(\-(?<pre>[0-9A-Za-z\-\.]+))?(\+(?<build>[0-9A-Za-z\-\.]+))?)\]\s+\-\s+(?<date>.*)
(?<notes>[^#]*)";
	Match match = Regex.Match(changesText, semverRegex);
	if (match.Success)
    {
		version = match.Groups["version"].Value;
        Information($"Publishing Version: {version}");
		notes = match.Groups["notes"].Value;
        Information($"With release notes: {notes}");
    }
	
	if(version == "0.0.0")
	{
		throw new Exception("No version number detected in changelog, please add a semantic version in the change log like so\n\n# [1.0.0-rc1] - 2017/03/13 \nAdded \n- Sweet temperature picker");
	}

	if(String.IsNullOrEmpty(notes))
	{
		throw new Exception($"No releate notes detected in changelog for version {version}, please add release notes e.g. \n\n# [1.1.0-rc1] - 2017-03-08 \nAdded \n- UpsertAll method");	
	}


    var nugetPackSettings = new NuGetPackSettings 
    { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./Build/nuget/",
		BasePath = "./",
		ReleaseNotes = new string[] {notes}
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
		
	// Push the package.
	NuGetPush(package, nugetPushSettings);
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
