#addin nuget:https://nuget.org/api/v2/?package=Cake.FileHelpers&version=1.0.3.2
#addin nuget:https://nuget.org/api/v2/?package=Cake.Xamarin&version=1.2.3
var TARGET = Argument ("target", Argument ("t", "Default"));

var isJenkinsBuild = Jenkins.IsRunningOnJenkins;
var pluginName = "Vanilla";
var packageName =  "Screenmedia.Plugin." + pluginName;
var sampleName =  pluginName + "Sample";

var libraries = new Dictionary<string, string> {
 	{ "./src/"+ packageName + ".sln", "Any" },
};

var samples = new Dictionary<string, string> {
	{ "./samples/" + sampleName + "/" + sampleName + ".sln", "Win" },
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

Task("Libraries").Does(()=>
{
    BuildAction(libraries);
});

Task("Samples")
    .IsDependentOn("Libraries")
    .Does(()=>
{
    BuildAction(samples);
});

Task ("NuGet")
	.IsDependentOn ("Libraries")
	.Does (() =>
{
    if(!DirectoryExists("./Build/nuget/"))
        CreateDirectory("./Build/nuget");
        
	var version = "0.0.0.9999";
	if(isJenkinsBuild)
	{
		version = EnvironmentVariable ("JENKINS_BUILD_VERSION")  ?? Argument("version", "0.0.0.9999");
	}
	else
	{
		version = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.0.9999");
	}
	NuGetPack ("./nuget/" + packageName + ".nuspec", new NuGetPackSettings { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./Build/nuget/",
		BasePath = "./",
	});	

    if(isJenkinsBuild)
    {
		// Get the path to the package.
		var package = "./Build/nuget/" + packageName + "." + version + ".nupkg";
	            
		// Push the package.
		NuGetPush(package, new NuGetPushSettings {
	    	Source = EnvironmentVariable("MYGET_SERVER"),
	    	ApiKey = EnvironmentVariable("MYGET_APIKEY")
		});
	}
});

//Build the build samples, nugets, and libraries
Task ("Default").IsDependentOn("NuGet");


Task ("Clean").Does (() => 
{
	CleanDirectories ("./Build/");
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});


RunTarget (TARGET);
