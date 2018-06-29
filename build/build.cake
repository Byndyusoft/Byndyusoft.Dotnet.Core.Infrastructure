using System.Text.RegularExpressions;

var target = Argument("Target", "Default");

var configuration = 
    HasArgument("Configuration") 
        ? Argument<string>("Configuration") 
        : EnvironmentVariable("Configuration") ?? "Release";

var buildNumber =
    HasArgument("BuildNumber") ? Argument<int>("BuildNumber") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number :
    TravisCI.IsRunningOnTravisCI ? TravisCI.Environment.Build.BuildNumber :
    EnvironmentVariable("BuildNumber") != null ? int.Parse(EnvironmentVariable("BuildNumber")) : 0;

var branch = 
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Repository.Branch :
    TravisCI.IsRunningOnTravisCI ? TravisCI.Environment.Build.Branch : (string)null;

string versionSuffix = null;
if(string.IsNullOrWhiteSpace(branch) == false && branch != "master")
{
    versionSuffix = $"-dev-build{buildNumber:00000}";

    var match = Regex.Match(branch, "release\\/\\d+\\.\\d+\\.\\d+\\-?(.*)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    if(match.Success)
        versionSuffix = string.IsNullOrWhiteSpace(match.Groups[1].Value) == false
            ? $"{match.Groups[1].Value}-build{buildNumber:00000}"
            : $"build{buildNumber:00000}";
}
 
var artifactsDirectory = MakeAbsolute(Directory("./artifacts"));
 
Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDirectory);
    });
 
 Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        var projects = GetFiles("../src/**/*.csproj").Concat(GetFiles("../tests/**/*.csproj"));
        var settings = new DotNetCoreBuildSettings
                {
                    Configuration = configuration,
                    VersionSuffix = versionSuffix
                };
        if(buildNumber != 0)
            settings.ArgumentCustomization = x => x.Append($"/p:Build={buildNumber}");

        foreach(var project in projects)
            DotNetCoreBuild(project.GetDirectory().FullPath, settings);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("../tests/**/*.csproj");
        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true,
        };

        foreach(var project in projects)
            DotNetCoreTest(project.FullPath, settings);
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var projects = GetFiles("../src/Infrastructure/**/*.csproj");
        var settings = new DotNetCorePackSettings
                {
                    Configuration = configuration,
                    NoRestore = true,
                    NoBuild = true,
                    OutputDirectory = artifactsDirectory,
                    IncludeSymbols = true,
                    VersionSuffix = versionSuffix
                };

        foreach (var project in projects)
            DotNetCorePack(project.GetDirectory().FullPath, settings);
    });

Task("Default")
    .IsDependentOn("Pack");
 
RunTarget(target);