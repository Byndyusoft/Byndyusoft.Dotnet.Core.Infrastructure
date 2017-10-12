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
var version = HasArgument("ShortVersion") ? Argument<string>("ShortVersion") : EnvironmentVariable("ShortVersion");
version = !string.IsNullOrWhiteSpace(version) ? version : "1.0.0";
var assemblyVersion = $"{version}.{buildNumber}";
var versionSuffix = HasArgument("VersionSuffix") ? Argument<string>("VersionSuffix") : EnvironmentVariable("VersionSuffix");
var packageVersion = version + (!string.IsNullOrWhiteSpace(versionSuffix) ? $"-{versionSuffix}-build{buildNumber}" : "");
 
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
        var projects = GetFiles("../src/**/*.csproj").Concat(GetFiles("../test/**/*.csproj"));
        foreach(var project in projects)
        {
           DotNetCoreBuild(
                project.GetDirectory().FullPath,
                new DotNetCoreBuildSettings()
                {
                    Configuration = configuration,
                    ArgumentCustomization = args => args
                        .Append($"/p:Version={version}")
                        .Append($"/p:AssemblyVersion={assemblyVersion}")
                });
        }
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("../test/**/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTool(
                project.FullPath,
                "xunit",
                new ProcessArgumentBuilder() 
                    .Append("-configuration " + configuration)
                    .Append("-nobuild")
                    .Append("-xml " + artifactsDirectory.CombineWithFilePath(project.GetFilenameWithoutExtension()).FullPath + ".xml")
                );
        }
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var projects = GetFiles("../src/Infrastructure/**/*.csproj");
        foreach (var project in projects)
        {
            DotNetCorePack(
                project.GetDirectory().FullPath,
                new DotNetCorePackSettings()
                {
                    Configuration = configuration,
                    NoBuild = true,
                    OutputDirectory = artifactsDirectory,
                    IncludeSymbols = true,
                     ArgumentCustomization = args => args
                        .Append($"/p:PackageVersion={packageVersion}")
                });
        }
    });

Task("Default")
    .IsDependentOn("Pack");
 
RunTarget(target);